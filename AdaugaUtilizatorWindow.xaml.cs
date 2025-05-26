using System.Windows;
using System.Windows.Controls;
using Npgsql;

namespace DentalProApp
{
    public partial class AdaugaUtilizatorWindow : Window
    {
        public AdaugaUtilizatorWindow()
        {
            InitializeComponent();
        }

        private void BtnSalveaza_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string parola = txtParola.Password.Trim();
            string rol = ((ComboBoxItem)cmbRol.SelectedItem)?.Content.ToString();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(parola) || string.IsNullOrWhiteSpace(rol))
            {
                MessageBox.Show("Completează toate câmpurile!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                string query = "INSERT INTO utilizator (username, parola_hash, rol) VALUES (@u, @p, @r)";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("u", username);
                    cmd.Parameters.AddWithValue("p", parola); // fără hash pentru test
                    cmd.Parameters.AddWithValue("r", rol);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Utilizator adăugat cu succes!");
                    this.DialogResult = true;
                    this.Close();
                }
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
