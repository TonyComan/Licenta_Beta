using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Npgsql;

namespace DentalProApp
{
    public partial class AdaugaFacturaWindow : Window
    {
        private Dictionary<int, string> pacienti = new Dictionary<int, string>();
        private Dictionary<int, decimal> tratamenteDisponibile = new Dictionary<int, decimal>();

        public AdaugaFacturaWindow()
        {
            InitializeComponent();
            IncarcaPacienti();
        }

        private void IncarcaPacienti()
        {
            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            string query = "SELECT id, nume, prenume FROM pacient ORDER BY nume";

            using (conn)
            {
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string numeComplet = reader.GetString(1) + " " + reader.GetString(2);
                            pacienti[id] = numeComplet;
                            ComboBoxItem item = new ComboBoxItem { Content = numeComplet, Tag = id };
                            cmbPacient.Items.Add(item);
                        }
                    }
                }
            }
        }

        private void cmbPacient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstTratamente.Items.Clear();
            tratamenteDisponibile.Clear();
            txtTotal.Text = "";

            ComboBoxItem selected = cmbPacient.SelectedItem as ComboBoxItem;
            if (selected == null)
                return;

            int pacientId = (int)selected.Tag;

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            string query = @"
                SELECT t.id,
                       s.denumire,
                       s.pret
                FROM tratament t
                JOIN serviciu_medical s ON t.serviciu_id = s.id
                WHERE t.pacient_id = @id AND t.id NOT IN (
                    SELECT tratament_id FROM factura_tratament
                )
                ORDER BY t.data_tratament DESC";

            using (conn)
            {
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("id", pacientId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int tratId = reader.GetInt32(0);
                            string serviciu = reader.GetString(1);
                            decimal pret = reader.GetDecimal(2);

                            tratamenteDisponibile[tratId] = pret;

                            ListBoxItem item = new ListBoxItem
                            {
                                Content = serviciu + " - " + pret.ToString("0.00") + " lei",
                                Tag = tratId
                            };
                            lstTratamente.Items.Add(item);
                        }
                    }
                }
            }

            lstTratamente.SelectionChanged += CalculeazaTotal;
        }

        private void CalculeazaTotal(object sender, SelectionChangedEventArgs e)
        {
            decimal total = 0;

            foreach (ListBoxItem item in lstTratamente.SelectedItems)
            {
                int id = (int)item.Tag;
                if (tratamenteDisponibile.ContainsKey(id))
                    total += tratamenteDisponibile[id];
            }

            txtTotal.Text = total.ToString("0.00");
        }

        private void BtnSalveaza_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedPacient = cmbPacient.SelectedItem as ComboBoxItem;
            ComboBoxItem selectedPlata = cmbMetodaPlata.SelectedItem as ComboBoxItem;

            if (selectedPacient == null || selectedPlata == null || lstTratamente.SelectedItems.Count == 0)
            {
                MessageBox.Show("Completează toate câmpurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            decimal total = decimal.Parse(txtTotal.Text);
            string metoda = selectedPlata.Content.ToString();

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                int facturaId;

                using (var cmd = new NpgsqlCommand("INSERT INTO factura (data_emitere, total, metoda_plata) VALUES (CURRENT_DATE, @t, @m) RETURNING id", conn))
                {
                    cmd.Parameters.AddWithValue("t", total);
                    cmd.Parameters.AddWithValue("m", metoda);
                    facturaId = (int)cmd.ExecuteScalar();
                }

                foreach (ListBoxItem item in lstTratamente.SelectedItems)
                {
                    int tratamentId = (int)item.Tag;

                    using (var cmd = new NpgsqlCommand("INSERT INTO factura_tratament (factura_id, tratament_id) VALUES (@f, @t)", conn))
                    {
                        cmd.Parameters.AddWithValue("f", facturaId);
                        cmd.Parameters.AddWithValue("t", tratamentId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("✅ Factura a fost creată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Eroare la creare factură:\n" + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
