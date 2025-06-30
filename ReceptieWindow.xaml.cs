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
using System.Windows.Threading;

namespace DentalProApp
{
    /// <summary>
    /// Interaction logic for ReceptieWindow.xaml
    /// </summary>
    public partial class ReceptieWindow : Window
    {
        public ReceptieWindow()
        {
            InitializeComponent();

            txtBunVenitReceptie.Text = $"Bun venit!";
            StartClock();
        }
        
        private void BtnPacienti_Click(object sender, RoutedEventArgs e)
        {
             
            var fereastra = new PacientiWindow();
            fereastra.ShowDialog();
        

    }

        private void BtnProgramari_Click(object sender, RoutedEventArgs e)
        {
            var fereastra = new ProgramariWindow();
            fereastra.ShowDialog();
        }

        private void BtnTratamente_Click(object sender, RoutedEventArgs e)
        {
            var fereastra = new TratamenteWindow("receptie");
            fereastra.Show();
        }

        private void BtnFacturi_Click(object sender, RoutedEventArgs e)
        {
            var fereastra = new FacturiWindow();
            fereastra.ShowDialog();
        }

        private void StartClock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {
                txtDataOraReceptie.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
            };
            timer.Start();
        }

        private void BtnDeconectare_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }




    }
}
