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
using DentalProApp.Models;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using Microsoft.Win32;


namespace DentalProApp
{
    /// <summary>
    /// Interaction logic for ColaborareWindow.xaml
    /// </summary>
    public partial class ColaborareWindow : Window
    {
        public ColaborareWindow()
        {
            InitializeComponent();
            LoadContracte();
        }

        private void LoadContracte()
        {
            List<ContractColaborare> contracte = new List<ContractColaborare>();

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            string query = @"
        SELECT c.id,
               CONCAT(m.nume, ' ', m.prenume) AS nume_medic,
               CONCAT(t.nume, ' ', t.prenume) AS nume_tehnician,
               t.email,
               t.telefon,
               t.specializare,
               c.durata_luni
        FROM contract_colaborare c
        JOIN medic m ON c.medic_id = m.id
        JOIN tehnician_dentar t ON c.tehnician_id = t.id
        ORDER BY c.id DESC";

            using (conn)
            using (var cmd = new NpgsqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    contracte.Add(new ContractColaborare
                    {
                        id = reader.GetInt32(0),
                        nume_medic = reader.GetString(1),
                        nume_tehnician = reader.GetString(2),
                        email_tehnician = reader.GetString(3),
                        telefon_tehnician = reader.GetString(4),
                        specializare_tehnician = reader.GetString(5),
                        durata_luni = reader.GetInt32(6)
                    });
                }
            }

            dgContracte.ItemsSource = contracte;
        }

        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {
            var fereastra = new AdaugaContractWindow();
            var rezultat = fereastra.ShowDialog();

            if (rezultat == true)
            {
                LoadContracte(); // Reîncarcă DataGrid-ul după adăugare
            }

        }
        private void BtnEditeaza_Click(object sender, RoutedEventArgs e)
        {
            var contract = dgContracte.SelectedItem as ContractColaborare;
            if (contract == null)
            {
                MessageBox.Show("Selectează un contract pentru editare.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var fereastra = new EditareContractWindow(contract.id);
            var rezultat = fereastra.ShowDialog();

            if (rezultat == true)
            {
                LoadContracte(); // Reîncarcă DataGrid-ul
            }

        }

        private void BtnSterge_Click(object sender, RoutedEventArgs e)
        {
            var contract = dgContracte.SelectedItem as ContractColaborare;
            if (contract == null)
            {
                MessageBox.Show("Selectează un contract de șters.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirmare = MessageBox.Show($"Ești sigur că vrei să ștergi contractul cu tehnicianul: {contract.nume_tehnician}?",
                                             "Confirmare", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmare == MessageBoxResult.Yes)
            {
                var conn = DbConnectionHelper.GetConnection();
                if (conn == null) return;

                try
                {
                    using (var cmd = new NpgsqlCommand("DELETE FROM contract_colaborare WHERE id = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("id", contract.id);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("✅ Contractul a fost șters.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadContracte(); // Reîncarcă tabela
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

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            var contract = dgContracte.SelectedItem as ContractColaborare;
            if (contract == null)
            {
                MessageBox.Show("Selectează un contract pentru export.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Extrage data_start din DB
            DateTime dataStart = DateTime.Now;
            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            using (conn)
            using (var cmd = new NpgsqlCommand("SELECT data_start FROM contract_colaborare WHERE id = @id", conn))
            {
                cmd.Parameters.AddWithValue("id", contract.id);
                var result = cmd.ExecuteScalar();
                if (result != null && result is DateTime)
                {
                    dataStart = (DateTime)result;
                }
            }

            DateTime dataSfarsit = dataStart.AddMonths(contract.durata_luni);

            // Creare document PDF
            var doc = new Document();
            var section = doc.AddSection();
            section.PageSetup.PageFormat = PageFormat.A4;
            section.PageSetup.TopMargin = Unit.FromCentimeter(2);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(2);
            section.PageSetup.RightMargin = Unit.FromCentimeter(2);

            doc.Styles["Normal"].Font.Name = "Segoe UI";
            doc.Styles["Normal"].Font.Size = 11;
            doc.Styles.AddStyle("Titlu", "Normal").Font.Size = 18;
            doc.Styles["Titlu"].Font.Bold = true;
            doc.Styles["Titlu"].ParagraphFormat.Alignment = ParagraphAlignment.Center;

            section.AddParagraph("CONTRACT DE COLABORARE", "Titlu").Format.SpaceAfter = "1cm";

            section.AddParagraph($"Contractul s-a încheiat între medicul: {contract.nume_medic} și tehnicianul dentar: {contract.nume_tehnician}.", "Normal");
            section.AddParagraph($"Specializare tehnician: {contract.specializare_tehnician}");
            section.AddParagraph($"Email: {contract.email_tehnician} | Telefon: {contract.telefon_tehnician}");
            section.AddParagraph($"Durata contractului: {contract.durata_luni} luni.");
            section.AddParagraph($"Data începerii: {dataStart:dd.MM.yyyy}");
            section.AddParagraph($"Data expirării: {dataSfarsit:dd.MM.yyyy}");

            section.AddParagraph("\n\nSemnături:", "Normal");
            section.AddParagraph("..............................................                                              ..............................................");
            section.AddParagraph("  ");
            section.AddParagraph("    Beneficiar (medic)                                                                              Colaborator (tehnician)");

            section.AddParagraph("\n\nDrepturile și obligațiile părților:", "Normal").Format.Font.Bold = true;
            section.AddParagraph("Părțile se angajează să colaboreze în conformitate cu normele profesionale și etice în vigoare. Tehnicianul se obligă să respecte termenele și standardele de calitate convenite.", "Normal");
            section.AddParagraph("În cazul în care una dintre părți nu respectă termenii contractului, cealaltă parte are dreptul să rezilieze contractul cu un preaviz de 30 de zile.", "Normal");

            var dialog = new SaveFileDialog
            {
                FileName = $"Contract_{contract.nume_tehnician.Replace(' ', '_')}.pdf",
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

                MessageBox.Show("✅ Contract exportat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }






        }
}
