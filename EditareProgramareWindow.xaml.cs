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
    public partial class EditareProgramareWindow : Window
    {
        private readonly Programare programare;

        public EditareProgramareWindow(Programare programareExistenta)
        {
            InitializeComponent();
            programare = programareExistenta;

            IncarcaPacienti();
            IncarcaMedici();
            IncarcaStatus();

            dpData.SelectedDate = programare.data_programare;
            txtOra.Text = programare.ora_programare.ToString(@"hh\:mm");
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
                    var item = new ComboBoxItem { Content = numeComplet, Tag = id };
                    cmbPacient.Items.Add(item);

                    if (id == programare.pacient_id)
                        cmbPacient.SelectedItem = item;
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
                    var item = new ComboBoxItem { Content = numeComplet, Tag = id };
                    cmbMedic.Items.Add(item);

                    if (id == programare.medic_id)
                        cmbMedic.SelectedItem = item;
                }
            }
        }

        private void IncarcaStatus()
        {
            foreach (ComboBoxItem item in cmbStatus.Items)
            {
                if (item.Content.ToString().ToLower() == programare.status.ToLower())
                {
                    cmbStatus.SelectedItem = item;
                    break;
                }
            }
        }

        private void BtnSalveaza_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPacient.SelectedItem == null || cmbMedic.SelectedItem == null || dpData.SelectedDate == null || cmbStatus.SelectedItem == null)
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
            string status = ((ComboBoxItem)cmbStatus.SelectedItem).Content.ToString();

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                string query = @"UPDATE programare SET
                                 pacient_id = @p,
                                 medic_id = @m,
                                 data_programare = @d,
                                 ora_programare = @o,
                                 status = @s
                                 WHERE id = @id";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("p", pacientId);
                    cmd.Parameters.AddWithValue("m", medicId);
                    cmd.Parameters.AddWithValue("d", data);
                    cmd.Parameters.AddWithValue("o", ora);
                    cmd.Parameters.AddWithValue("s", status);
                    cmd.Parameters.AddWithValue("id", programare.id);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("✅ Programare actualizată cu succes!");
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