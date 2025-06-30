using Npgsql;
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

namespace DentalProApp
{
    /// <summary>
    /// Interaction logic for EditareConsimtamantWindow.xaml
    /// </summary>
    public partial class EditareConsimtamantWindow : Window
    {
        public Consimtamant ConsimtamantDeEditat { get; set; }
        public EditareConsimtamantWindow(Consimtamant consimtamant)
        {
            InitializeComponent();
            ConsimtamantDeEditat = consimtamant;
            dpData.SelectedDate = consimtamant.data_document;
            txtDescriere.Text = consimtamant.descriere;
            cmbPacienti.Text = consimtamant.nume_pacient;
            txtCNP.Text = consimtamant.cnp;
            txtTelefon.Text = consimtamant.telefon;
            cmbMedici.Text = consimtamant.nume_medic;
        }

        private void BtnSalveaza_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescriere.Text) || dpData.SelectedDate == null)
            {
                MessageBox.Show("Completează toate câmpurile!", "Avertisment", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var descriereNoua = txtDescriere.Text.Trim();
            var dataNoua = dpData.SelectedDate.Value;

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                string updateQuery = "UPDATE consimtamant_pacient SET descriere = @d, data_document = @dt WHERE id = @id";
                using (var cmd = new NpgsqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("d", descriereNoua);
                    cmd.Parameters.AddWithValue("dt", dataNoua);
                    cmd.Parameters.AddWithValue("id", ConsimtamantDeEditat.id);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Modificările au fost salvate cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la salvare: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }



    }
}
