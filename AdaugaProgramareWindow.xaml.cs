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
    public partial class AdaugaProgramareWindow : Window
    {
        public AdaugaProgramareWindow()
        {
            InitializeComponent();
            IncarcaPacienti();
            IncarcaMedici();
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

        private void BtnSalveaza_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPacient.SelectedItem == null || cmbMedic.SelectedItem == null || dpData.SelectedDate == null)
            {
                MessageBox.Show("Completează toate câmpurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TimeSpan.TryParse(txtOra.Text.Trim(), out TimeSpan ora))
            {
                MessageBox.Show("Ora nu este într-un format valid (hh:mm).", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int pacientId = (int)((ComboBoxItem)cmbPacient.SelectedItem).Tag;
            int medicId = (int)((ComboBoxItem)cmbMedic.SelectedItem).Tag;
            DateTime data = dpData.SelectedDate.Value;

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                string query = @"INSERT INTO programare (pacient_id, medic_id, data_programare, ora_programare, status)
                                 VALUES (@p, @m, @d, @o, 'activ')";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("p", pacientId);
                    cmd.Parameters.AddWithValue("m", medicId);
                    cmd.Parameters.AddWithValue("d", data);
                    cmd.Parameters.AddWithValue("o", ora);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("✅ Programare adăugată cu succes!");
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
