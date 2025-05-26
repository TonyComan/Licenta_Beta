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
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            LoadUsers(); // Incarca pacientii la deschidere
        }

        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcție Adăugare utilizator (în lucru)");

            var fereastra = new AdaugaUtilizatorWindow();
            var rezultat = fereastra.ShowDialog();

            if (rezultat == true)
            {
                LoadUsers(); // reincarca lista dupa adaugare
            }
        }

        private void BtnEditeaza_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcție Editare utilizator (în lucru)");


            var utilizatorSelectat = dgUtilizatori.SelectedItem as Utilizator;

            if (utilizatorSelectat == null)
            {
                MessageBox.Show("Selectează un utilizator pentru editare.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var fereastra = new EditareUtilizatorWindow(utilizatorSelectat);
            var rezultat = fereastra.ShowDialog();

            if (rezultat == true)
            {
                LoadUsers();
            }
        }

        private void BtnSterge_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcție Ștergere utilizator (în lucru)");


            var utilizatorSelectat = dgUtilizatori.SelectedItem as Utilizator;

            if (utilizatorSelectat == null)
            {
                MessageBox.Show("Selectează un utilizator pentru ștergere.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirmare = MessageBox.Show(
                $"Ești sigur că vrei să ștergi utilizatorul \"{utilizatorSelectat.username}\"?",
                "Confirmare ștergere",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirmare != MessageBoxResult.Yes)
                return;

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                string query = "DELETE FROM utilizator WHERE id = @id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("id", utilizatorSelectat.id);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show(" Utilizator șters cu succes.");
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la ștergere: " + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        



        private void LoadUsers()
        {
            var utilizatori = new List<Utilizator>();

            string query = "SELECT id, username, rol FROM utilizator ORDER BY id";

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
                            utilizatori.Add(new Utilizator
                            {
                                id = reader.GetInt32(0),
                                username = reader.GetString(1),
                                rol = reader.GetString(2)
                            });
                        }
                    }
                }
            }
            dgUtilizatori.ItemsSource = utilizatori;



        }
    }
}


    
