using System.Windows;
using Npgsql;

namespace DentalProApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim(); // parola hashuită în viitor

            string query = "SELECT rol FROM utilizator WHERE username = @u AND parola_hash = @p";

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            try
            {
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("u", username);
                    cmd.Parameters.AddWithValue("p", password); // doar pt test, ulterior criptăm

                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string rol = result.ToString();
                        MessageBox.Show($"Bun venit, {username}! Rol: {rol}");

                        // În funcție de rol, deschide o fereastră diferită
                        Window fereastra = null;

                        if (rol == "admin") fereastra = new AdminWindow();
                        else if (rol == "medic") fereastra = new MedicWindow();
                        else if (rol == "receptie") fereastra = new ReceptieWindow();

                        if (fereastra != null)
                        {
                            fereastra.Show();
                            Application.Current.MainWindow = fereastra;
                            this.Close();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Utilizator sau parolă incorectă.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            finally
            {
                conn.Close();
            }

        }
    }
}