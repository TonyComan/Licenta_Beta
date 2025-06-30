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
    public partial class AdaugaContractWindow : Window
    {
        public AdaugaContractWindow()
        {
            InitializeComponent();
            IncarcaMedici();
            IncarcaTehnicieni();
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
                using (var cmd = new NpgsqlCommand("INSERT INTO contract_colaborare (medic_id, tehnician_id, durata_luni) VALUES (@m, @t, @d)", conn))
                {
                    cmd.Parameters.AddWithValue("m", medicId);
                    cmd.Parameters.AddWithValue("t", tehnicianId);
                    cmd.Parameters.AddWithValue("d", durataLuni);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("✅ Contract salvat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
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
