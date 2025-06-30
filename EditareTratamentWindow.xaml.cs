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
    public partial class EditareTratamentWindow : Window
    {
        private readonly Tratament tratament;

        public EditareTratamentWindow(Tratament tratamentDeEditat)
        {
            InitializeComponent();
            tratament = tratamentDeEditat;

            IncarcaPacienti();
            IncarcaMedici();
            IncarcaServicii();

            dpData.SelectedDate = tratament.data_tratament;
            txtDescriere.Text = tratament.descriere;
            txtObservatii.Text = tratament.observatii;
        }

        private void IncarcaPacienti()
        {
            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            string query = "SELECT id, nume, prenume FROM pacient ORDER BY nume";

            using (conn)
            using (var cmd = new NpgsqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string nume = $"{reader.GetString(1)} {reader.GetString(2)}";
                    var item = new ComboBoxItem { Content = nume, Tag = id };
                    cmbPacient.Items.Add(item);

                    if (id == tratament.pacient_id)
                        cmbPacient.SelectedItem = item;
                }
            }
        }

        private void IncarcaMedici()
        {
            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            string query = "SELECT id, nume, prenume FROM medic ORDER BY nume";

            using (conn)
            using (var cmd = new NpgsqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string nume = $"{reader.GetString(1)} {reader.GetString(2)}";
                    var item = new ComboBoxItem { Content = nume, Tag = id };
                    cmbMedic.Items.Add(item);

                    if (id == tratament.medic_id)
                        cmbMedic.SelectedItem = item;
                }
            }
        }

        private void IncarcaServicii()
        {
            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            string query = "SELECT id, denumire FROM serviciu_medical ORDER BY denumire";

            using (conn)
            using (var cmd = new NpgsqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string denumire = reader.GetString(1);
                    var item = new ComboBoxItem { Content = denumire, Tag = id };
                    cmbServiciu.Items.Add(item);

                    if (id == tratament.serviciu_id)
                        cmbServiciu.SelectedItem = item;
                }
            }
        }

        private void BtnSalveaza_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPacient.SelectedItem == null || cmbMedic.SelectedItem == null || cmbServiciu.SelectedItem == null || dpData.SelectedDate == null)
            {
                MessageBox.Show("Completează toate câmpurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int pacientId = (int)((ComboBoxItem)cmbPacient.SelectedItem).Tag;
            int medicId = (int)((ComboBoxItem)cmbMedic.SelectedItem).Tag;
            int serviciuId = (int)((ComboBoxItem)cmbServiciu.SelectedItem).Tag;
            DateTime data = dpData.SelectedDate.Value;
            string descriere = txtDescriere.Text.Trim();
            string observatii = txtObservatii.Text.Trim();

            if (string.IsNullOrWhiteSpace(descriere))
            {
                MessageBox.Show("Descrierea este obligatorie!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                string query = @"UPDATE tratament SET
                                 pacient_id = @p,
                                 medic_id = @m,
                                 serviciu_id = @s,
                                 descriere = @d,
                                 data_tratament = @dt,
                                 observatii = @o
                                 WHERE id = @id";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("p", pacientId);
                    cmd.Parameters.AddWithValue("m", medicId);
                    cmd.Parameters.AddWithValue("s", serviciuId);
                    cmd.Parameters.AddWithValue("d", descriere);
                    cmd.Parameters.AddWithValue("dt", data);
                    cmd.Parameters.AddWithValue("o", observatii);
                    cmd.Parameters.AddWithValue("id", tratament.id);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("✅ Tratamentul a fost actualizat cu succes!");
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Eroare la actualizare: " + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
