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
    /// <summary>
    /// Interaction logic for PacientiWindow.xaml
    /// </summary>
    public partial class PacientiWindow : Window
    {
        public PacientiWindow()
        {
            InitializeComponent();
            LoadPacienti(); // Incarca pacientii la deschidere
        }

        private void LoadPacienti()
        {
            var pacienti = new List<Pacient>();

            string query = "SELECT id, nume, prenume, cnp, data_nasterii, telefon, email, adresa FROM pacient ORDER BY id";

            var conn = DbConnectionHelper.GetConnection();
            if (conn == null) return;

            using (conn)
            {
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pacienti.Add(new Pacient
                            {
                                id = reader.GetInt32(0),
                                nume = reader.GetString(1),
                                prenume = reader.GetString(2),
                                cnp = reader.GetString(3),
                                data_nasterii = reader.GetDateTime(4),
                                telefon = reader.GetString(5),
                                email = reader.GetString(6),
                                adresa = reader.GetString(7)
                            });
                        }
                    }
                }
            }

            dgPacienti.ItemsSource = pacienti;
        }
    }
}


