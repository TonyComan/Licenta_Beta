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
    /// Interaction logic for ProgramariWindow.xaml
    /// </summary>
    public partial class ProgramariWindow : Window
    {
        public ProgramariWindow()
        {
            InitializeComponent();
            LoadProgramari();
        }

        


        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcție Adăugare programare (în lucru)");
            var fereastra = new AdaugaProgramareWindow();
            var rezultat = fereastra.ShowDialog();

            if (rezultat == true)
            {
                LoadProgramari(); // Reîncarcă lista după salvare
            }
        }

        private void BtnEditeaza_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcție Editare programare (în lucru)");
            var programareSelectata = dgProgramari.SelectedItem as Programare;

            if (programareSelectata == null)
            {
                MessageBox.Show("Selectează o programare pentru editare.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var fereastra = new EditareProgramareWindow(programareSelectata);
            var rezultat = fereastra.ShowDialog();

            if (rezultat == true)
            {
                LoadProgramari(); // Reîncarcă după salvare
            }
        }

        private void BtnSterge_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcție Ștergere programare (în lucru)");
            var programareSelectata = dgProgramari.SelectedItem as Programare;

            if (programareSelectata == null)
            {
                MessageBox.Show("Selectează o programare pentru ștergere.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirmare = MessageBox.Show(
                $"Ești sigur că vrei să ștergi programarea pacientului \"{programareSelectata.nume_pacient}\" din {programareSelectata.data_programare:dd.MM.yyyy} ora {programareSelectata.ora_programare}?",
                "Confirmare ștergere",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirmare != MessageBoxResult.Yes)
                return;

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                string query = "DELETE FROM programare WHERE id = @id";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("id", programareSelectata.id);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("✅ Programare ștearsă cu succes!");
                LoadProgramari();
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


        

        private void LoadProgramari()
        {
            var programari = new List<Programare>();

            string query = @"
        SELECT 
            p.id,
            p.pacient_id,
            CONCAT(pac.nume, ' ', pac.prenume) AS nume_pacient,
            p.medic_id,
            CONCAT(m.nume, ' ', m.prenume) AS nume_medic,
            p.data_programare,
            p.ora_programare,
            p.status
        FROM programare p
        JOIN pacient pac ON p.pacient_id = pac.id
        JOIN medic m ON p.medic_id = m.id
        ORDER BY p.data_programare, p.ora_programare";

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
                            programari.Add(new Programare
                            {
                                id = reader.GetInt32(0),
                                pacient_id = reader.GetInt32(1),
                                nume_pacient = reader.GetString(2),
                                medic_id = reader.GetInt32(3),
                                nume_medic = reader.GetString(4),
                                data_programare = reader.GetDateTime(5),
                                ora_programare = reader.GetTimeSpan(6),
                                status = reader.GetString(7)
                            });
                        }
                    }
                }
            }

            //dgProgramari.ItemsSource = programari;
            toateProgramarile = programari;
            dgProgramari.ItemsSource = toateProgramarile;
        }

        private List<Programare> toateProgramarile = new List<Programare>();
        private void txtCautareProgramari_TextChanged(object sender, TextChangedEventArgs e)
        {
            ComboBoxItem selectedItem = cmbFiltruProgramari.SelectedItem as ComboBoxItem;
            if (selectedItem == null) return;

            string criteriu = selectedItem.Content.ToString();
            string text = txtCautareProgramari.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(text))
            {
                dgProgramari.ItemsSource = toateProgramarile;
                return;
            }

            var filtrat = toateProgramarile.Where(p =>
                (criteriu == "Nume pacient" && p.nume_pacient.ToLower().Contains(text)) ||
                (criteriu == "Nume medic" && p.nume_medic.ToLower().Contains(text)) ||
                (criteriu == "Data programării" && p.data_programare.ToString("dd.MM.yyyy").Contains(text)) ||
                (criteriu == "Status" && p.status.ToLower().Contains(text))
            ).ToList();

            dgProgramari.ItemsSource = filtrat;
        }

    }
}
