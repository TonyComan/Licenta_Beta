using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Npgsql;

namespace DentalProApp
{
    public partial class EditareFacturaWindow : Window
    {
        private readonly Factura factura;

        public EditareFacturaWindow(Factura facturaDeEditat)
        {
            InitializeComponent();
            factura = facturaDeEditat;

            IncarcaTratamentSelectat();
            PreselecteazaDate();
        }

        private void IncarcaTratamentSelectat()
        {
            // tratamentul nu se poate schimba, dar îl afișăm
            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            string query = @"
                SELECT CONCAT(p.nume, ' ', p.prenume), s.denumire
                FROM tratament t
                JOIN pacient p ON t.pacient_id = p.id
                LEFT JOIN serviciu_medical s ON t.serviciu_id = s.id
                WHERE t.id = @id";

            using (conn)
            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("id", factura.tratament_id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string pacient = reader.GetString(0);
                        string serviciu = reader.IsDBNull(1) ? "-" : reader.GetString(1);
                        string display = $"{pacient} - {serviciu}";

                        cmbTratament.Items.Add(new ComboBoxItem { Content = display, Tag = factura.tratament_id });
                        cmbTratament.SelectedIndex = 0;
                    }
                }
            }
        }

        private void PreselecteazaDate()
        {
            txtTotal.Text = factura.total.ToString("0.00");

            foreach (ComboBoxItem item in cmbMetodaPlata.Items)
            {
                if (item.Content.ToString().Equals(factura.metoda_plata, StringComparison.OrdinalIgnoreCase))
                {
                    cmbMetodaPlata.SelectedItem = item;
                    break;
                }
            }
        }

        private void BtnSalveaza_Click(object sender, RoutedEventArgs e)
        {
            if (cmbMetodaPlata.SelectedItem == null || string.IsNullOrWhiteSpace(txtTotal.Text))
            {
                MessageBox.Show("Completează toate câmpurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtTotal.Text.Trim(), out decimal totalNou) || totalNou <= 0)
            {
                MessageBox.Show("Suma totală nu este validă!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string metodaNoua = ((ComboBoxItem)cmbMetodaPlata.SelectedItem).Content.ToString();

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                string query = @"UPDATE factura
                                 SET total = @total, metoda_plata = @metoda
                                 WHERE id = @id";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("total", totalNou);
                    cmd.Parameters.AddWithValue("metoda", metodaNoua);
                    cmd.Parameters.AddWithValue("id", factura.id);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("✅ Factura a fost actualizată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Eroare la actualizare: " + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
