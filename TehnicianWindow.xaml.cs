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
    public partial class TehnicianWindow : Window
    {
        public TehnicianWindow()
        {
            InitializeComponent();
            LoadTehnicieni();
        }

        private void LoadTehnicieni()
        {
            var tehnicieni = new List<Tehnician>();

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            string query = "SELECT id, nume, prenume, specializare, telefon, email FROM tehnician_dentar ORDER BY id";

            using (conn)
            using (var cmd = new NpgsqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tehnicieni.Add(new Tehnician
                    {
                        id = reader.GetInt32(0),
                        nume = reader.GetString(1),
                        prenume = reader.GetString(2),
                        specializare = reader.GetString(3),
                        telefon = reader.GetString(4),
                        email = reader.GetString(5)
                    });
                }
            }

            dgTehnicieni.ItemsSource = tehnicieni;
        }

        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {
            var fereastra = new AdaugaTehnicianWindow();
            var rezultat = fereastra.ShowDialog();
            if (rezultat == true)
            {
                LoadTehnicieni(); // actualizează lista
            }
        }

        private void BtnEditeaza_Click(object sender, RoutedEventArgs e)
        {
            var tehnician = dgTehnicieni.SelectedItem as Tehnician;
            if (tehnician == null)
            {
                MessageBox.Show("Selectează un tehnician pentru editare.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var fereastra = new EditareTehnicianWindow(tehnician);
            var rezultat = fereastra.ShowDialog();

            if (rezultat == true)
            {
                LoadTehnicieni(); // Reîncarcă lista
            }
        }

        private void BtnSterge_Click(object sender, RoutedEventArgs e)
        {
            var tehnician = dgTehnicieni.SelectedItem as Tehnician;
            if (tehnician == null)
            {
                MessageBox.Show("Selectează un tehnician pentru a-l șterge.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirmare = MessageBox.Show(
                $"Ești sigur că vrei să ștergi tehnicianul:\n{tehnician.nume} {tehnician.prenume}?",
                "Confirmare",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (confirmare == MessageBoxResult.Yes)
            {
                var conn = DbConnectionHelper.GetConnection();
                if (conn == null) return;

                try
                {
                    using (var cmd = new NpgsqlCommand("DELETE FROM tehnician_dentar WHERE id = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("id", tehnician.id);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("✅ Tehnicianul a fost șters cu succes.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadTehnicieni(); // reîncarcă lista după ștergere
                }
                catch (Exception ex)
                {
                    MessageBox.Show("❌ Eroare la ștergere:\n" + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}

