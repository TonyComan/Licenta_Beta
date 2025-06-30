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
    /// <summary>
    /// Interaction logic for PacientiWindow.xaml
    /// </summary>
    public partial class PacientiWindow : Window
    {
        public PacientiWindow()
        {
            InitializeComponent();
            LoadPacienti(); // Incarca pacientii la deschidere
        }
        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcție Adăugare pacient (în lucru)");
            var fereastra = new AdaugaPacientWindow();
            var rezultat = fereastra.ShowDialog();

            if (rezultat == true)
            {
                LoadPacienti(); // Reîncarcă lista după adăugare
            }
        }

        private void BtnEditeaza_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcție Editare pacient (în lucru)");

            var pacientSelectat = dgPacienti.SelectedItem as Pacient;

            if (pacientSelectat == null)
            {
                MessageBox.Show("Selectează un pacient pentru editare.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var fereastra = new EditarePacientWindow(pacientSelectat);
            var rezultat = fereastra.ShowDialog();

            if (rezultat == true)
            {
                LoadPacienti();
            }
        }

        private void BtnSterge_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcție Ștergere pacient (în lucru)");
            var pacientSelectat = dgPacienti.SelectedItem as Pacient;

            if (pacientSelectat == null)
            {
                MessageBox.Show("Selectează un pacient pentru ștergere.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirmare = MessageBox.Show(
                $"Ești sigur că vrei să ștergi pacientul \"{pacientSelectat.nume} {pacientSelectat.prenume}\"?",
                "Confirmare ștergere",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirmare != MessageBoxResult.Yes)
                return;

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                string query = "DELETE FROM pacient WHERE id = @id";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("id", pacientSelectat.id);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("✅ Pacient șters cu succes.");
                LoadPacienti(); // Reîncarcă lista
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Eroare la ștergere: " + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                conn.Close();
            }
        }
    

        

        private void LoadPacienti()
        {
            var pacienti = new List<Pacient>();

            string query = "SELECT id, nume, prenume, cnp, data_nasterii, telefon, email, adresa FROM pacient ORDER BY id";

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            using (conn)
            {
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pacienti.Add(new Pacient
                            {
                                id = reader.GetInt32(0),
                                nume = reader.GetString(1),
                                prenume = reader.GetString(2),
                                cnp = reader.GetString(3),
                                data_nasterii = reader.GetDateTime(4),
                                telefon = reader.GetString(5),
                                email = reader.GetString(6),
                                adresa = reader.GetString(7)
                            });
                        }
                    }
                }
            }

            dgPacienti.ItemsSource = pacienti;
        }



        private void txtCautare_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dgPacienti.ItemsSource == null) return;

            var criteriu = (cmbCriteriu.SelectedItem as ComboBoxItem)?.Content.ToString();
            var text = txtCautare.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(criteriu) || string.IsNullOrEmpty(text))
            {
                dgPacienti.Items.Filter = null;
                return;
            }

            dgPacienti.Items.Filter = item =>
            {
                var pacient = item as Pacient;
                if (pacient == null) return false;

                switch (criteriu)
                {
                    case "Nume":
                        return pacient.nume != null && pacient.nume.ToLower().Contains(text);
                    case "CNP":
                        return pacient.cnp != null && pacient.cnp.ToLower().Contains(text);
                    case "Telefon":
                        return pacient.telefon != null && pacient.telefon.ToLower().Contains(text);
                    case "Email":
                        return pacient.email != null && pacient.email.ToLower().Contains(text);
                    default:
                        return true;
                }
            };
        }



    }
}


