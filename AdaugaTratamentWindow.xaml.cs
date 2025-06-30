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
    public partial class AdaugaTratamentWindow : Window
    {
        public AdaugaTratamentWindow()
        {
            InitializeComponent();
            IncarcaPacienti();
            IncarcaMedici();
            IncarcaServicii();
            dpData.SelectedDate = DateTime.Today;
        }

        private void IncarcaPacienti()
        {
            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            string query = "SELECT id, nume, prenume FROM pacient ORDER BY nume";

            using (conn)
            using (var cmd = new NpgsqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string numeComplet = $"{reader.GetString(1)} {reader.GetString(2)}";
                    cmbPacient.Items.Add(new ComboBoxItem { Content = numeComplet, Tag = id });
                }
            }
        }

        private void IncarcaMedici()
        {
            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            string query = "SELECT id, nume, prenume FROM medic ORDER BY nume";

            using (conn)
            using (var cmd = new NpgsqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string numeComplet = $"{reader.GetString(1)} {reader.GetString(2)}";
                    cmbMedic.Items.Add(new ComboBoxItem { Content = numeComplet, Tag = id });
                }
            }
        }

        private void IncarcaServicii()
        {
            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            string query = "SELECT id, denumire FROM serviciu_medical ORDER BY denumire";

            using (conn)
            using (var cmd = new NpgsqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string denumire = reader.GetString(1);
                    cmbServiciu.Items.Add(new ComboBoxItem { Content = denumire, Tag = id });
                }
            }
        }

        private void BtnSalveaza_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPacient.SelectedItem == null || cmbMedic.SelectedItem == null || cmbServiciu.SelectedItem == null || dpData.SelectedDate == null)
            {
                MessageBox.Show("Te rugăm să completezi toate câmpurile obligatorii!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int pacientId = (int)((ComboBoxItem)cmbPacient.SelectedItem).Tag;
            int medicId = (int)((ComboBoxItem)cmbMedic.SelectedItem).Tag;
            int serviciuId = (int)((ComboBoxItem)cmbServiciu.SelectedItem).Tag;
            DateTime data = dpData.SelectedDate.Value;

            string descriere = txtDescriere.Text.Trim();
            string observatii = txtObservatii.Text.Trim();

            if (string.IsNullOrWhiteSpace(descriere))
            {
                MessageBox.Show("Descrierea tratamentului este obligatorie!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                string query = @"INSERT INTO tratament (pacient_id, medic_id, serviciu_id, descriere, data_tratament, observatii)
                                 VALUES (@p, @m, @s, @d, @dt, @o)";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("p", pacientId);
                    cmd.Parameters.AddWithValue("m", medicId);
                    cmd.Parameters.AddWithValue("s", serviciuId);
                    cmd.Parameters.AddWithValue("d", descriere);
                    cmd.Parameters.AddWithValue("dt", data);
                    cmd.Parameters.AddWithValue("o", observatii);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("✅ Tratament adăugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Eroare la salvare: " + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}