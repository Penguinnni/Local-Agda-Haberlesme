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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LocalHaberlesmeSinifKutuphanesi;

namespace LocalHaberlesme
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {

        private SunucuBaglan sunBag;
        private SunucuOlustur sunOls;
        public MainWindow()
        {
            InitializeComponent();
            sunBag = new SunucuBaglan(this);
            sunOls = new SunucuOlustur(this);
            Pencere.Navigate(sunBag);
        }
        








        private void Kapatbyn_Click(object sender, RoutedEventArgs e)// Pencere Kapat
        {
            this.Close();
        }

        private void KucultBtn_Click(object sender, RoutedEventArgs e)// Pencereyi simge durumna küçült
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Ustcubuk_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)// Pencereyi hareket ettir
        {
            this.DragMove();
        }

        private void Olustursyfbtn_Click(object sender, RoutedEventArgs e)// Sunucu oluştur sayfasını gösterir
        {
            Pencere.Navigate(sunOls);
        }

        private void Baglansyfbtn_Click(object sender, RoutedEventArgs e)// Sunucuya bağlan sayfasını gösterir
        {
            Pencere.Navigate(sunBag);
        }
    }
}
