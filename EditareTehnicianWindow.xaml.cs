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
    public partial class EditareTehnicianWindow : Window
    {
        private int tehnicianId;

        public EditareTehnicianWindow(Tehnician tehnician)
        {
            InitializeComponent();
            tehnicianId = tehnician.id;

            // Precompletăm câmpurile
            txtNume.Text = tehnician.nume;
            txtPrenume.Text = tehnician.prenume;
            txtSpecializare.Text = tehnician.specializare;
            txtTelefon.Text = tehnician.telefon;
            txtEmail.Text = tehnician.email;
        }

        private void BtnSalveaza_Click(object sender, RoutedEventArgs e)
        {
            string nume = txtNume.Text.Trim();
            string prenume = txtPrenume.Text.Trim();
            string specializare = txtSpecializare.Text.Trim();
            string telefon = txtTelefon.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(nume) ||
                string.IsNullOrWhiteSpace(prenume) ||
                string.IsNullOrWhiteSpace(specializare) ||
                string.IsNullOrWhiteSpace(telefon) ||
                string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Completează toate câmpurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                using (var cmd = new NpgsqlCommand(
                    "UPDATE tehnician_dentar SET nume=@n, prenume=@p, specializare=@s, telefon=@t, email=@e WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("n", nume);
                    cmd.Parameters.AddWithValue("p", prenume);
                    cmd.Parameters.AddWithValue("s", specializare);
                    cmd.Parameters.AddWithValue("t", telefon);
                    cmd.Parameters.AddWithValue("e", email);
                    cmd.Parameters.AddWithValue("id", tehnicianId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("✅ Tehnicianul a fost actualizat!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Eroare la actualizare:\n" + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
