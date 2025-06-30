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
using Microsoft.Win32;
using PdfSharp.Pdf;
using Microsoft.SqlServer.Server;

namespace DentalProApp
{
    /// <summary>
    /// Interaction logic for ConsimtamantPacientWindow.xaml
    /// </summary>
    public partial class ConsimtamantPacientWindow : Window
    {
        public ConsimtamantPacientWindow()
        {
            InitializeComponent();
            LoadConsimtamante();
        }


        private void LoadConsimtamante()
        {
            var lista = new List<Consimtamant>();

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                string query = @"
            SELECT c.id, 
                   p.nume || ' ' || p.prenume AS nume_pacient,
                   p.cnp, 
                   p.telefon,
                   m.nume || ' ' || m.prenume AS nume_medic,
                   c.data_document,
                   c.descriere
            FROM consimtamant_pacient c
            JOIN pacient p ON c.pacient_id = p.id
            JOIN medic m ON c.medic_id = m.id
            ORDER BY c.data_document DESC;
        ";

                 var cmd = new NpgsqlCommand(query, conn);
                 var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Consimtamant
                    {
                        id = reader.GetInt32(0),
                        nume_pacient = reader.GetString(1),
                        cnp = reader.GetString(2),
                        telefon = reader.GetString(3),
                        nume_medic = reader.GetString(4),
                        data_document = reader.GetDateTime(5),
                        descriere = reader.GetString(6)
                    });
                }

                dgConsimtamant.ItemsSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Eroare la încărcarea datelor:\n" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }






        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {
            var fereastra = new AdaugaConsimtamantWindow();
            var rezultat = fereastra.ShowDialog();

            if (rezultat == true)
            {
                LoadConsimtamante(); //reincarca lista automat
            }
        }

        private void BtnEditeaza_Click(object sender, RoutedEventArgs e)
        {
            if (dgConsimtamant.SelectedItem is Consimtamant selectat)
            {
                var fereastra = new EditareConsimtamantWindow(selectat);
                if (fereastra.ShowDialog() == true)
                {
                    LoadConsimtamante(); // Reîncarcă datele
                }
            }
            else
            {
                MessageBox.Show("Selectează un consimțământ pentru a edita.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void BtnSterge_Click(object sender, RoutedEventArgs e)
        {
            var selectat = dgConsimtamant.SelectedItem as Consimtamant;
            if (selectat == null)
            {
                MessageBox.Show("Selectează un consimțământ pentru a-l șterge.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirmare = MessageBox.Show(
                $"Ești sigur că vrei să ștergi consimțământul pacientului {selectat.nume_pacient}?",
                "Confirmare ștergere", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmare != MessageBoxResult.Yes)
                return;

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                var cmd = new NpgsqlCommand("DELETE FROM consimtamant_pacient WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("id", selectat.id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("✅ Consimțământul a fost șters.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadConsimtamante();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Eroare la ștergere:\n" + ex.Message);
            }
            finally
            {
                conn.Close();
            }

        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            var consimtamant = dgConsimtamant.SelectedItem as Consimtamant;
            if (consimtamant == null)
            {
                MessageBox.Show("Selectează un consimțământ pentru a-l exporta.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var document = new Document();
            var section = document.AddSection();

            section.PageSetup.TopMargin = Unit.FromCentimeter(2);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(2);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(2.5);
            section.PageSetup.RightMargin = Unit.FromCentimeter(2.5);

            var title = section.AddParagraph("Consimțământul Pacientului pentru Intervenția Chirurgicală");
            title.Format.Font.Size = 16;
            title.Format.Font.Bold = true;
            title.Format.Alignment = ParagraphAlignment.Center;
            title.Format.SpaceAfter = "1cm";

            section.AddParagraph($"Data consimțământului: {consimtamant.data_document:dd.MM.yyyy}")
                   .Format.SpaceAfter = "0.5cm";

            section.AddParagraph($"Pacient: {consimtamant.nume_pacient}")
                   .Format.SpaceAfter = "0.2cm";
            section.AddParagraph($"CNP: {consimtamant.cnp}")
                   .Format.SpaceAfter = "0.2cm";
            section.AddParagraph($"Telefon: {consimtamant.telefon}")
                   .Format.SpaceAfter = "0.5cm";

            section.AddParagraph($"Medic responsabil: {consimtamant.nume_medic}")
                   .Format.SpaceAfter = "0.5cm";

            section.AddParagraph("Descriere intervenție:")
                   .Format.Font.Bold = true;
            section.AddParagraph(consimtamant.descriere)
                   .Format.SpaceAfter = "1cm";

            section.AddParagraph("Semnătura pacientului: __________________________")
                   .Format.SpaceAfter = "0.5cm";
            section.AddParagraph("Semnătura medicului: __________________________")
                   .Format.SpaceAfter = "1cm";
            

            // Text final preluat din documentul tău
            var mesajFinal = "Prin semnarea acestui document, pacientul declară că a fost informat complet " +
                             "și înțeles toate aspectele legate de intervenția propusă, riscurile implicate și alternativele posibile. " +
                             "Consimțământul este dat în mod liber și informat." ;
            
            

            var pFinal = section.AddParagraph(mesajFinal);
            pFinal.Format.Alignment = ParagraphAlignment.Justify;
            pFinal.Format.Font.Size = 11;

            var renderer = new PdfDocumentRenderer(true)
            {
                Document = document
            };

            renderer.RenderDocument();

            var filename = $"Consimtamant_{consimtamant.nume_pacient.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var saveDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = $"Consimtamant_{consimtamant.nume_pacient.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}",
                DefaultExt = ".pdf",
                Filter = "PDF documents (.pdf)|*.pdf"
            };

            if (saveDialog.ShowDialog() == true)
            {
                renderer.PdfDocument.Save(saveDialog.FileName);

                MessageBox.Show("✅ PDF exportat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }

         }




    }
}
