using System.Windows;
using Npgsql;

namespace DentalProApp
{
    public partial class EditareUtilizatorWindow : Window
    {
        private readonly Utilizator utilizator;

        public EditareUtilizatorWindow(Utilizator u)
        {
            InitializeComponent();
            utilizator = u;

            txtUsername.Text = u.username;
            cmbRol.ItemsSource = new[] { "admin", "medic", "receptie" };
            cmbRol.SelectedItem = u.rol;
        }

        private void BtnSalveaza_Click(object sender, RoutedEventArgs e)
        {
            string usernameNou = txtUsername.Text.Trim();
            string parolaNoua = txtParola.Password.Trim(); // poate fi lăsată goală
            string rolNou = cmbRol.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(usernameNou) || string.IsNullOrWhiteSpace(rolNou))
            {
                MessageBox.Show("Completează toate câmpurile obligatorii!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                string query;
                var cmd = new NpgsqlCommand();
                cmd.Connection = conn;

                if (string.IsNullOrWhiteSpace(parolaNoua))
                {
                    query = "UPDATE utilizator SET username = @u, rol = @r WHERE id = @id";
                    cmd.Parameters.AddWithValue("u", usernameNou);
                    cmd.Parameters.AddWithValue("r", rolNou);
                }
                else
                {
                    query = "UPDATE utilizator SET username = @u, parola_hash = @p, rol = @r WHERE id = @id";
                    cmd.Parameters.AddWithValue("u", usernameNou);
                    cmd.Parameters.AddWithValue("p", parolaNoua); // hashing mai târziu
                    cmd.Parameters.AddWithValue("r", rolNou);
                }

                cmd.Parameters.AddWithValue("id", utilizator.id);
                cmd.CommandText = query;

                cmd.ExecuteNonQuery();

                MessageBox.Show("✅ Utilizator actualizat!");
                this.DialogResult = true;
                this.Close();
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
