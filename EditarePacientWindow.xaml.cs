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
    public partial class EditarePacientWindow : Window
    {
        private readonly Pacient pacientDeEditat;

        public EditarePacientWindow(Pacient pacient)
        {
            InitializeComponent();
            pacientDeEditat = pacient;

            // Precompletare câmpuri
            txtNume.Text = pacient.nume;
            txtPrenume.Text = pacient.prenume;
            txtCNP.Text = pacient.cnp;
            dpDataNasterii.SelectedDate = pacient.data_nasterii;
            txtTelefon.Text = pacient.telefon;
            txtEmail.Text = pacient.email;
            txtAdresa.Text = pacient.adresa;
        }

        private void BtnSalveaza_Click(object sender, RoutedEventArgs e)
        {
            string nume = txtNume.Text.Trim();
            string prenume = txtPrenume.Text.Trim();
            string cnp = txtCNP.Text.Trim();
            DateTime? dataNasterii = dpDataNasterii.SelectedDate;
            string telefon = txtTelefon.Text.Trim();
            string email = txtEmail.Text.Trim();
            string adresa = txtAdresa.Text.Trim();

            if (string.IsNullOrWhiteSpace(nume) ||
                string.IsNullOrWhiteSpace(prenume) ||
                string.IsNullOrWhiteSpace(cnp) ||
                !dataNasterii.HasValue ||
                string.IsNullOrWhiteSpace(telefon) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(adresa))
            {
                MessageBox.Show("Completează toate câmpurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                string query = @"UPDATE pacient SET
                                    nume = @n,
                                    prenume = @p,
                                    cnp = @c,
                                    data_nasterii = @d,
                                    telefon = @t,
                                    email = @e,
                                    adresa = @a
                                WHERE id = @id";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("n", nume);
                    cmd.Parameters.AddWithValue("p", prenume);
                    cmd.Parameters.AddWithValue("c", cnp);
                    cmd.Parameters.AddWithValue("d", dataNasterii.Value);
                    cmd.Parameters.AddWithValue("t", telefon);
                    cmd.Parameters.AddWithValue("e", email);
                    cmd.Parameters.AddWithValue("a", adresa);
                    cmd.Parameters.AddWithValue("id", pacientDeEditat.id);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("✅ Pacient actualizat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
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
