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


namespace DentalProApp
{
    /// <summary>
    /// Interaction logic for FacturiWindow.xaml
    /// </summary>
    public partial class FacturiWindow : Window
    {
        public FacturiWindow()
        {
            InitializeComponent();
            LoadFacturi();
        }

        private List<Factura> toateFacturile = new List<Factura>();

        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {
            var fereastra = new AdaugaFacturaWindow();
            var rezultat = fereastra.ShowDialog();

            if (rezultat == true)
            {
                LoadFacturi(); // reîncarcă lista după adăugare
            }
        }


        private void BtnEditeaza_Click(object sender, RoutedEventArgs e)
        {
            var facturaSelectata = dgFacturi.SelectedItem as Factura;

            if (facturaSelectata == null)
            {
                MessageBox.Show("Selectează o factură pentru editare.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var fereastra = new EditareFacturaWindow(facturaSelectata);
            var rezultat = fereastra.ShowDialog();

            if (rezultat == true)
            {
                LoadFacturi(); // reîncarcă lista
            }


        }


        private void BtnSterge_Click(object sender, RoutedEventArgs e)
        {
            var facturaSelectata = dgFacturi.SelectedItem as Factura;

            if (facturaSelectata == null)
            {
                MessageBox.Show("Selectează o factură pentru ștergere.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirmare = MessageBox.Show(
                $"Ești sigur că vrei să ștergi factura cu ID #{facturaSelectata.id} pentru pacientul {facturaSelectata.nume_pacient}?",
                "Confirmare ștergere",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirmare != MessageBoxResult.Yes)
                return;

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                string query = "DELETE FROM factura WHERE id = @id";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("id", facturaSelectata.id);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("✅ Factura a fost ștearsă cu succes.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadFacturi(); // reîncarcă după ștergere
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
            var factura = dgFacturi.SelectedItem as Factura;
            if (factura == null)
            {
                MessageBox.Show("Selectează o factură pentru export.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Extragem tratamentele facturii
            var tratamente = new List<string>();
            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            string query = @"
        SELECT s.denumire, s.pret
        FROM factura_tratament ft
        JOIN tratament t ON ft.tratament_id = t.id
        JOIN serviciu_medical s ON t.serviciu_id = s.id
        WHERE ft.factura_id = @id";

            using (conn)
            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("id", factura.id);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string denumire = reader.GetString(0);
                        decimal pret = reader.GetDecimal(1);
                        tratamente.Add($"{denumire} - {pret:0.00} lei");
                    }
                }
            }

            // Generăm documentul PDF
            var doc = new Document();
            var section = doc.AddSection();

            section.PageSetup.PageFormat = PageFormat.A4;
            section.PageSetup.TopMargin = Unit.FromCentimeter(2);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(2);
            section.PageSetup.RightMargin = Unit.FromCentimeter(2);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(2);

            doc.Styles["Normal"].Font.Name = "Segoe UI";
            doc.Styles["Normal"].Font.Size = 11;

            section.AddParagraph("FACTURĂ MEDICALĂ", "Heading1").Format.Alignment = ParagraphAlignment.Center;
            section.AddParagraph($"Număr factură: #{factura.id}", "BoldLine");
            section.AddParagraph($"Pacient: {factura.nume_pacient}", "Line");
            section.AddParagraph($"Medic: {factura.nume_medic}", "Line");
            section.AddParagraph($"Dată emitere: {factura.data_emitere:dd.MM.yyyy}", "Line");
            section.AddParagraph($"Metodă de plată: {factura.metoda_plata}", "Line");

            section.AddParagraph("Tratamente incluse:", "BoldLine");

            if (tratamente.Count > 0)
            {
                foreach (var trat in tratamente)
                {
                    section.AddParagraph("• " + trat, "Line");
                }
            }
            else
            {
                section.AddParagraph("• Niciun tratament înregistrat", "Line");
            }

            section.AddParagraph("\nTotal de plată: " + factura.total.ToString("0.00") + " lei", "BoldLine");

            section.AddParagraph("----------------------------------------------------------").Format.SpaceBefore = "1cm";
            section.AddParagraph("Factura generată electronic de sistemul DentalPro.\nVă mulțumim pentru încredere!", "Info");

            doc.Styles.AddStyle("Line", "Normal").ParagraphFormat.SpaceAfter = "0.3cm";
            doc.Styles.AddStyle("BoldLine", "Normal").Font.Bold = true;
            doc.Styles["BoldLine"].ParagraphFormat.SpaceAfter = "0.5cm";
            doc.Styles.AddStyle("Info", "Normal").Font.Size = 10;
            doc.Styles["Info"].ParagraphFormat.SpaceBefore = "0.5cm";
            doc.Styles.AddStyle("Heading1", "Normal").Font.Size = 18;
            doc.Styles["Heading1"].Font.Bold = true;
            doc.Styles["Heading1"].ParagraphFormat.SpaceAfter = "1cm";

            var dialog = new SaveFileDialog
            {
                FileName = $"Factura_{factura.nume_pacient.Replace(' ', '_')}_{factura.data_emitere:yyyyMMdd}.pdf",
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

                MessageBox.Show("✅ Factura a fost exportată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void BtnFiltrare_Click(object sender, RoutedEventArgs e)
        {

            string text = txtCautare.Text.Trim().ToLower();
            string criteriu = (cmbFiltru.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(criteriu))
            {
                dgFacturi.ItemsSource = toateFacturile;
                return;
            }

            var rezultate = toateFacturile.Where(f =>
            {
                switch (criteriu)
                {
                    case "Pacient":
                        return f.nume_pacient.ToLower().Contains(text);
                    case "Medic":
                        return f.nume_medic.ToLower().Contains(text);
                    case "Serviciu":
                        return f.nume_serviciu.ToLower().Contains(text);
                    case "Metodă plată":
                        return f.metoda_plata.ToLower().Contains(text);
                    case "Data":
                        return f.data_emitere.ToString("dd.MM.yyyy").Contains(text);
                    case "Toate câmpurile":
                    default:
                        return f.nume_pacient.ToLower().Contains(text)
                            || f.nume_medic.ToLower().Contains(text)
                            || f.nume_serviciu.ToLower().Contains(text)
                            || f.metoda_plata.ToLower().Contains(text)
                            || f.data_emitere.ToString("dd.MM.yyyy").Contains(text);
                }
            }).ToList();

            dgFacturi.ItemsSource = rezultate;



        }

        private void LoadFacturi()
        {
            var facturi = new List<Factura>();

            string query = @"
        SELECT f.id,
       CONCAT(p.nume, ' ', p.prenume) AS nume_pacient,
       CONCAT(m.nume, ' ', m.prenume) AS nume_medic,
       STRING_AGG(s.denumire, ', ') AS nume_serviciu,
       f.data_emitere,
       f.total,
       f.metoda_plata
FROM factura f
JOIN factura_tratament ft ON f.id = ft.factura_id
JOIN tratament t ON ft.tratament_id = t.id
JOIN pacient p ON t.pacient_id = p.id
JOIN medic m ON t.medic_id = m.id
LEFT JOIN serviciu_medical s ON t.serviciu_id = s.id
GROUP BY f.id, p.nume, p.prenume, m.nume, m.prenume, f.data_emitere, f.total, f.metoda_plata
ORDER BY f.data_emitere DESC";

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            using (conn)
            using (var cmd = new NpgsqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    facturi.Add(new Factura
                    {
                        id = reader.GetInt32(0),
                        nume_pacient = reader.GetString(1),
                        nume_medic = reader.GetString(2),
                        nume_serviciu = reader.IsDBNull(3) ? "-" : reader.GetString(3),
                        data_emitere = reader.GetDateTime(4),
                        total = reader.GetDecimal(5),
                        metoda_plata = reader.GetString(6)
                    });

                }
            }

            
            toateFacturile = facturi;
            dgFacturi.ItemsSource = facturi;

        }

    }
}
