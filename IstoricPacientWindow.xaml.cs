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
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using Npgsql;

namespace DentalProApp
{
    public partial class IstoricPacientWindow : Window
    {
        public IstoricPacientWindow()
        {
            InitializeComponent();
        }
        private Pacient pacientSelectat;
        private List<TratamenteIstoric> tratamenteList = new List<TratamenteIstoric>();

        private void BtnCauta_Click(object sender, RoutedEventArgs e)
        {
            string numeCautat = txtCautare.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(numeCautat))
            {
                MessageBox.Show("Introduceți un nume pentru căutare.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                var cmd = new NpgsqlCommand(@"
            SELECT 
                p.id, p.nume, p.prenume, p.cnp, p.telefon, p.email,
                s.denumire AS denumire_tratament,
                t.data_tratament, 
                m.nume || ' ' || m.prenume AS nume_medic,
                s.denumire AS serviciu,
                t.observatii
            FROM pacient p
            LEFT JOIN tratament t ON p.id = t.pacient_id
            LEFT JOIN medic m ON t.medic_id = m.id
            LEFT JOIN serviciu_medical s ON t.serviciu_id = s.id
            WHERE LOWER(p.nume || ' ' || p.prenume) LIKE @numeComplet
            ORDER BY t.data_tratament;", conn);

                cmd.Parameters.AddWithValue("numeComplet", $"%{numeCautat}%");

                var reader = cmd.ExecuteReader();

                tratamenteList.Clear();
                pacientSelectat = null;

                bool pacientGasit = false;

                while (reader.Read())
                {
                    if (!pacientGasit)
                    {
                        pacientGasit = true;

                        pacientSelectat = new Pacient
                        {
                            id = reader.GetInt32(0),
                            nume = reader.GetString(1),
                            prenume = reader.GetString(2),
                            cnp = reader.IsDBNull(3) ? "-" : reader.GetString(3),
                            telefon = reader.IsDBNull(4) ? "-" : reader.GetString(4),
                            email = reader.IsDBNull(5) ? "-" : reader.GetString(5)
                        };
                    }

                    if (!reader.IsDBNull(6))
                    {
                        tratamenteList.Add(new TratamenteIstoric
                        {
                            denumire_tratament = reader.GetString(6),
                            data = reader.GetDateTime(7),
                            nume_medic = reader.IsDBNull(8) ? "-" : reader.GetString(8),
                            serviciu = reader.IsDBNull(9) ? "-" : reader.GetString(9),
                            observatii = reader.IsDBNull(10) ? "-" : reader.GetString(10)
                        });
                    }
                }

                reader.Close();

                if (!pacientGasit)
                {
                    MessageBox.Show("Pacientul nu a fost găsit în sistem.", "Informație", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                txtInfoPacient.Text = $"📋 {pacientSelectat.nume} {pacientSelectat.prenume} — CNP: {pacientSelectat.cnp} — Tel: {pacientSelectat.telefon} — Email: {pacientSelectat.email}";
                itemsTratamente.ItemsSource = tratamenteList;
                txtRezumat.Text = $"Număr total de tratamente: {tratamenteList.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea istoricului:\n" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void BtnExportPDF_Click(object sender, RoutedEventArgs e)
        {
            if (pacientSelectat == null || tratamenteList == null || tratamenteList.Count == 0)
            {
                MessageBox.Show("Nu există suficiente date pentru export.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var doc = new Document();
            var section = doc.AddSection();
            section.PageSetup.TopMargin = Unit.FromCentimeter(2);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(2);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(2.5);
            section.PageSetup.RightMargin = Unit.FromCentimeter(2.5);

            // Titlu
            var title = section.AddParagraph(" Istoric Pacient");
            title.Format.Font.Size = 16;
            title.Format.Font.Bold = true;
            title.Format.Alignment = ParagraphAlignment.Center;
            title.Format.SpaceAfter = "1cm";

            // Date personale
            section.AddParagraph($"Nume: {pacientSelectat.nume} {pacientSelectat.prenume}");
            section.AddParagraph($"CNP: {pacientSelectat.cnp}");
            section.AddParagraph($"Telefon: {pacientSelectat.telefon}");
            section.AddParagraph($"Email: {pacientSelectat.email}").Format.SpaceAfter = "1cm";

            // Lista tratamente
            foreach (var t in tratamenteList)
            {
                var tratamentPar = section.AddParagraph();
                tratamentPar.AddFormattedText("Tratament efectuat", TextFormat.Bold);
                tratamentPar.Format.SpaceAfter = "0.2cm";

                section.AddParagraph($"• Denumire: {t.denumire_tratament}");
                section.AddParagraph($"• Dată: {t.data:dd.MM.yyyy}");
                section.AddParagraph($"• Medic: {t.nume_medic}");
                section.AddParagraph($"• Serviciu: {t.serviciu}");
                section.AddParagraph($"• Observații: {t.observatii}").Format.SpaceAfter = "0.8cm";
            }

            // Rezumat
            section.AddParagraph($"📄 Număr total de tratamente: {tratamenteList.Count}")
                   .Format.Font.Bold = true;

            var renderer = new PdfDocumentRenderer(true)
            {
                Document = doc
            };
            renderer.RenderDocument();

            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = $"Istoric_{pacientSelectat.nume}_{pacientSelectat.prenume}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                Filter = "PDF file (*.pdf)|*.pdf"
            };

            if (dialog.ShowDialog() == true)
            {
                renderer.PdfDocument.Save(dialog.FileName);
                MessageBox.Show("📄 PDF exportat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtCautare.Clear();
            txtInfoPacient.Text = "";
            txtRezumat.Text = "";
            itemsTratamente.ItemsSource = null;
            pacientSelectat = null;
            tratamenteList.Clear();
        }


    }
}
