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
using System.Windows.Controls.Primitives;
using LocalHaberlesmeSinifKutuphanesi.Istemci;

namespace LocalHaberlesme
{
    /// <summary>
    /// OzelSohbet.xaml etkileşim mantığı
    /// </summary>
    public partial class OzelSohbet : Page
    {
        public Popup p;
        public string kullaniciAdi = null;
        Istemci istemci;
        public OzelSohbet(Popup popup,Istemci istemci)
        {
            InitializeComponent();
            p = popup;
            this.istemci = istemci;
        }

        public void OzelSohbetYenile(string kullaniciAdi,string mesajlar)
        {
            this.kullaniciAdi = kullaniciAdi;
            kAdi.Content = kullaniciAdi;
            Sohbet.Text = mesajlar;
        }

        private void Kapat_Click(object sender, RoutedEventArgs e)
        {
            p.IsOpen = false;
        }

        private void Temizlebuton_Click(object sender, RoutedEventArgs e)
        {
            Sohbet.Text = null;
        }

        private void OzelMesajGonder()
        {
            if (IstemciSinifi.MesajGonderebilmeDurum)
            {
                bool kontrol = false;
                foreach (char karakter in Mesaj.Text.ToCharArray())
                {
                    if ((int)karakter != 32)
                    {
                        kontrol = true;
                        break;
                    }
                }

                if (kontrol)
                {
                    istemci.OzelMesajGonder(Mesaj.Text);
                    Mesaj.Text = null;
                }
            }
        }

        public void EkranaYazdir(string mesaj)
        {
            this.Dispatcher.Invoke(() =>
            {
                Sohbet.Text = Sohbet.Text + DateTime.Now.ToShortTimeString() +" - "+ mesaj;
            });
        }

        private void Mesaj_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OzelMesajGonder();
            }
        }

        private void Gonderbuton_Click(object sender, RoutedEventArgs e)
        {
            OzelMesajGonder();
        }

        private void TuruncuCerceve_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void Mesaj_TextChanged(object sender, TextChangedEventArgs e)
        {
            Mesaj.ScrollToEnd();
        }

        private void Sohbet_TextChanged(object sender, TextChangedEventArgs e)
        {
            Sohbet.ScrollToEnd();
        }
    }
}
