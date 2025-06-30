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
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System.IO;
using Microsoft.Win32;


namespace DentalProApp
{
    /// <summary>
    /// Interaction logic for TratamenteWindow.xaml
    /// </summary>
    public partial class TratamenteWindow : Window
    {
        public TratamenteWindow()
        {
            InitializeComponent();
            LoadTratamente();
        }

        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Adăugare tratament (în lucru)");
            var fereastra = new AdaugaTratamentWindow();
            var rezultat = fereastra.ShowDialog();

            if (rezultat == true)
            {
                LoadTratamente(); // Reîncarcă după adăugare
            }
        }

        private void BtnEditeaza_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Editare tratament (în lucru)");
            var tratamentSelectat = dgTratamente.SelectedItem as Tratament;

            if (tratamentSelectat == null)
            {
                MessageBox.Show("Selectează un tratament pentru editare.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var fereastra = new EditareTratamentWindow(tratamentSelectat);
            var rezultat = fereastra.ShowDialog();

            if (rezultat == true)
            {
                LoadTratamente(); // reîncarcă lista
            }
        }

        private void BtnSterge_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ștergere tratament (în lucru)");
            var tratamentSelectat = dgTratamente.SelectedItem as Tratament;

            if (tratamentSelectat == null)
            {
                MessageBox.Show("Selectează un tratament pentru ștergere.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirmare = MessageBox.Show(
                $"Ești sigur că vrei să ștergi tratamentul pacientului \"{tratamentSelectat.nume_pacient}\" din {tratamentSelectat.data_tratament:dd.MM.yyyy}?",
                "Confirmare ștergere",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirmare != MessageBoxResult.Yes)
                return;

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                string query = "DELETE FROM tratament WHERE id = @id";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("id", tratamentSelectat.id);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("✅ Tratament șters cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadTratamente(); // Reîncarcă lista după ștergere
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

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Export PDF (în lucru)");
            var tratament = dgTratamente.SelectedItem as Tratament;

            if (tratament == null)
            {
                MessageBox.Show("Selectează un tratament pentru export.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var doc = new Document();
            var section = doc.AddSection();

            // Setări fonturi globale
            section.PageSetup.PageFormat = PageFormat.A4;
            section.PageSetup.LeftMargin = Unit.FromCentimeter(2);
            section.PageSetup.RightMargin = Unit.FromCentimeter(2);
            section.PageSetup.TopMargin = Unit.FromCentimeter(2);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(2);
            section.PageSetup.PageFormat = PageFormat.A4;

            var title = section.AddParagraph("FIȘĂ DE TRATAMENT");
            title.Format.Font.Size = 18;
            title.Format.Font.Bold = true;
            title.Format.Font.Name = "Arial";
            title.Format.SpaceAfter = "1cm";
            title.Format.Alignment = ParagraphAlignment.Center;

            // Info principal
            section.AddParagraph($" Pacient: {tratament.nume_pacient}", "InfoStyle");
            section.AddParagraph($" Medic: {tratament.nume_medic}", "InfoStyle");
            section.AddParagraph($" Serviciu medical: {tratament.nume_serviciu}", "InfoStyle");
            section.AddParagraph($" Data tratamentului: {tratament.data_tratament:dd.MM.yyyy}", "InfoStyle");
            section.AddParagraph("\n");

            // Descriere
            var descriereTitle = section.AddParagraph("📋 Descrierea tratamentului:");
            descriereTitle.Format.Font.Bold = true;
            descriereTitle.Format.Font.Size = 12;
            descriereTitle.Format.SpaceAfter = "0.3cm";

            section.AddParagraph(tratament.descriere + "\n", "TextNormal");

            // Observații
            var obsTitle = section.AddParagraph(" Observații:");
            obsTitle.Format.Font.Bold = true;
            obsTitle.Format.Font.Size = 12;
            obsTitle.Format.SpaceAfter = "0.3cm";

            section.AddParagraph(string.IsNullOrWhiteSpace(tratament.observatii)
                ? "Nu au fost menționate observații suplimentare.\n"
                : tratament.observatii + "\n", "TextNormal");

            // Umplere pagină cu text fictiv (ex: notițe interne)
            section.AddParagraph("────────────────────────────────────────────────────────────").Format.SpaceBefore = "1cm";
            section.AddParagraph("Notițe interne și completări:\n\n" +
                "Prin generarea acestei fișe de tratament, se confirmă faptul că pacientului i-a fost aplicat un serviciu medical inclus în lista procedurilor stomatologice aprobate de către cabinetul DentalPro SRL, conform reglementărilor interne și standardelor profesionale în vigoare. " +
                "Fișa are scop informativ și medical, certificând intervenția efectuată, numele medicului responsabil, data prestării serviciului, precum și recomandările post-tratament. " +
                "Pacientul este informat prin prezenta cu privire la natura tratamentului realizat și la eventualele controale ulterioare necesare, asumându-și, prin acceptarea serviciului, responsabilitatea respectării indicațiilor medicale oferite. " 
                ).Format.Font.Size = 10;

            // Stiluri
            var style = doc.Styles["Normal"];
            style.Font.Name = "Segoe UI";
            style.Font.Size = 11;

            doc.Styles.AddStyle("InfoStyle", "Normal").Font.Size = 12;
            doc.Styles["InfoStyle"].ParagraphFormat.SpaceAfter = "0.3cm";

            doc.Styles.AddStyle("TextNormal", "Normal").Font.Size = 11;
            doc.Styles["TextNormal"].ParagraphFormat.SpaceAfter = "0.5cm";

            // Salvează
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = $"FisaTratament_{tratament.nume_pacient.Replace(' ', '_')}_{tratament.data_tratament:yyyyMMdd}.pdf",
                Filter = "Fișiere PDF (*.pdf)|*.pdf"
            };

            if (dialog.ShowDialog() == true)
            {
                var renderer = new PdfDocumentRenderer
                {
                    Document = doc
                };
                renderer.RenderDocument();
                renderer.Save(dialog.FileName);

                MessageBox.Show("✅ Fișa de tratament a fost exportată cu succes!", "Export PDF", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }


        private void LoadTratamente()
        {
            var tratamente = new List<Tratament>();

            string query = @"
        SELECT t.id,
               t.pacient_id,
               CONCAT(p.nume, ' ', p.prenume) AS nume_pacient,
               t.medic_id,
               CONCAT(m.nume, ' ', m.prenume) AS nume_medic,
               t.serviciu_id,
               s.denumire AS nume_serviciu,
               t.descriere,
               t.data_tratament,
               t.observatii
        FROM tratament t
        JOIN pacient p ON t.pacient_id = p.id
        JOIN medic m ON t.medic_id = m.id
        LEFT JOIN serviciu_medical s ON t.serviciu_id = s.id
        ORDER BY t.data_tratament DESC";

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            using (conn)
            using (var cmd = new NpgsqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tratamente.Add(new Tratament
                    {
                        id = reader.GetInt32(0),
                        pacient_id = reader.GetInt32(1),
                        nume_pacient = reader.GetString(2),
                        medic_id = reader.GetInt32(3),
                        nume_medic = reader.GetString(4),
                        serviciu_id = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                        nume_serviciu = reader.IsDBNull(6) ? "-" : reader.GetString(6),
                        descriere = reader.GetString(7),
                        data_tratament = reader.GetDateTime(8),
                        observatii = reader.IsDBNull(9) ? "" : reader.GetString(9)
                    });
                }
            }

            dgTratamente.ItemsSource = tratamente;
        }

        public TratamenteWindow(string rolUtilizator)
        {
            InitializeComponent();

            if (rolUtilizator == "medic")
            {
                btnExport.Visibility = Visibility.Collapsed;
            }
            else if (rolUtilizator == "receptie")
            {
                btnAdauga.Visibility = Visibility.Collapsed;
            }

            LoadTratamente(); // sau metoda ta de încărcare
        }
    }
}
