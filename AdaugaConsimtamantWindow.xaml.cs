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
    public partial class AdaugaConsimtamantWindow : Window
    {
        private Dictionary<string, (int id, string cnp, string telefon)> pacientiMap = new Dictionary<string, (int id, string cnp, string telefon)>();
        private Dictionary<string, int> mediciMap = new Dictionary<string, int>();

        public AdaugaConsimtamantWindow()
        {
            InitializeComponent();
            dpData.SelectedDate = DateTime.Today;
            IncarcaPacienti();
            IncarcaMedici();
        }

        private void IncarcaPacienti()
        {
            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                 var cmd = new NpgsqlCommand("SELECT id, nume || ' ' || prenume AS nume, cnp, telefon FROM pacient ORDER BY nume", conn);
                 var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var nume = reader.GetString(1);
                    cmbPacienti.Items.Add(nume);
                    pacientiMap[nume] = (reader.GetInt32(0), reader.GetString(2), reader.GetString(3));
                }
            }
            finally
            {
                conn.Close();
            }
        }


        private void IncarcaMedici()
        {
            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                var cmd = new NpgsqlCommand("SELECT id, nume || ' ' || prenume AS nume_complet FROM medic ORDER BY nume", conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string numeComplet = reader.GetString(1);
                    cmbMedici.Items.Add(numeComplet);
                    mediciMap[numeComplet] = reader.GetInt32(0);
                }
            }
            finally
            {
                conn.Close();
            }
        }

        private void cmbPacienti_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbPacienti.SelectedItem is string nume && pacientiMap.TryGetValue(nume, out var info))
            {
                txtCNP.Text = info.cnp;
                txtTelefon.Text = info.telefon;
            }
        }

        private void BtnSalveaza_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPacienti.SelectedItem == null || string.IsNullOrWhiteSpace(cmbMedici.Text) || string.IsNullOrWhiteSpace(txtDescriere.Text))
            {
                MessageBox.Show("Completează toate câmpurile obligatorii.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var pacientId = pacientiMap[cmbPacienti.SelectedItem.ToString()].id;
            if (cmbMedici.SelectedItem == null)
            {
                MessageBox.Show("Selectează un medic.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var medicId = mediciMap[cmbMedici.SelectedItem.ToString()];
            var descriere = txtDescriere.Text.Trim();
            var data = dpData.SelectedDate ?? DateTime.Today;

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                 var cmd = new NpgsqlCommand("INSERT INTO consimtamant_pacient (pacient_id, medic_id, descriere, data_document)\r\nVALUES (@p, @m, @d, @data)", conn);
                cmd.Parameters.AddWithValue("p", pacientId);
                cmd.Parameters.AddWithValue("m", medicId);
                cmd.Parameters.AddWithValue("d", descriere);
                cmd.Parameters.AddWithValue("data", data);
                cmd.ExecuteNonQuery();

                MessageBox.Show("✅ Consimțământul a fost salvat!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Eroare la salvare:\n" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
