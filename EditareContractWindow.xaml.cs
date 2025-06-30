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
    public partial class EditareContractWindow : Window
    {
        private int contractId;

        public EditareContractWindow(int id)
        {
            InitializeComponent();
            contractId = id;

            IncarcaMedici();
            IncarcaTehnicieni();
            IncarcaContract(contractId);
        }

        private void IncarcaMedici()
        {
            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            using (conn)
            using (var cmd = new NpgsqlCommand("SELECT id, nume, prenume FROM medic ORDER BY nume", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string numeComplet = reader.GetString(1) + " " + reader.GetString(2);

                    ComboBoxItem item = new ComboBoxItem
                    {
                        Content = numeComplet,
                        Tag = id
                    };
                    cmbMedici.Items.Add(item);
                }
            }
        }

        private void IncarcaTehnicieni()
        {
            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            using (conn)
            using (var cmd = new NpgsqlCommand("SELECT id, nume, prenume FROM tehnician_dentar ORDER BY nume", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string numeComplet = reader.GetString(1) + " " + reader.GetString(2);

                    ComboBoxItem item = new ComboBoxItem
                    {
                        Content = numeComplet,
                        Tag = id
                    };
                    cmbTehnicieni.Items.Add(item);
                }
            }
        }

        private void IncarcaContract(int id)
        {
            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            string query = "SELECT medic_id, tehnician_id, durata_luni FROM contract_colaborare WHERE id = @id";

            using (conn)
            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int medicId = reader.GetInt32(0);
                        int tehnicianId = reader.GetInt32(1);
                        int durata = reader.GetInt32(2);

                        txtDurata.Text = durata.ToString();

                        foreach (ComboBoxItem item in cmbMedici.Items)
                        {
                            if ((int)item.Tag == medicId)
                            {
                                cmbMedici.SelectedItem = item;
                                break;
                            }
                        }

                        foreach (ComboBoxItem item in cmbTehnicieni.Items)
                        {
                            if ((int)item.Tag == tehnicianId)
                            {
                                cmbTehnicieni.SelectedItem = item;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void BtnSalveaza_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem medicItem = cmbMedici.SelectedItem as ComboBoxItem;
            ComboBoxItem tehnicianItem = cmbTehnicieni.SelectedItem as ComboBoxItem;

            if (medicItem == null || tehnicianItem == null || string.IsNullOrWhiteSpace(txtDurata.Text))
            {
                MessageBox.Show("Completează toate câmpurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtDurata.Text.Trim(), out int durataLuni) || durataLuni <= 0)
            {
                MessageBox.Show("Durata trebuie să fie un număr valid pozitiv.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int medicId = (int)medicItem.Tag;
            int tehnicianId = (int)tehnicianItem.Tag;

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                using (var cmd = new NpgsqlCommand("UPDATE contract_colaborare SET medic_id=@m, tehnician_id=@t, durata_luni=@d WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("m", medicId);
                    cmd.Parameters.AddWithValue("t", tehnicianId);
                    cmd.Parameters.AddWithValue("d", durataLuni);
                    cmd.Parameters.AddWithValue("id", contractId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("✅ Modificările au fost salvate!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Eroare la salvare:\n" + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
