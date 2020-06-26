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
using LocalHaberlesmeSinifKutuphanesi.Istemci;
using System.Net;

namespace LocalHaberlesme
{
    /// <summary>
    /// SunucuBaglan.xaml etkileşim mantığı
    /// </summary>
    public partial class SunucuBaglan : Page
    {
        Window mw;
        public SunucuBaglan(Window mw)
        {
            InitializeComponent();
            Ortak.OrtakSinif.kAdi_IP_PORTDoldur(kullaniciAdiTb, ipAdresTb, portTb);
            this.mw = mw;
        }

        private void BaglanBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                int port = int.Parse(portTb.Text);

                Ortak.OrtakSinif.KullaniciAdiKontrol(kullaniciAdiTb.Text);
                Ortak.OrtakSinif.PortveIpKontrol(ipAdresTb.Text, port);

                IstemciSinifi.Baglan(kullaniciAdiTb.Text, IPAddress.Parse(ipAdresTb.Text), port);

                Istemci istemciPencere = new Istemci(kullaniciAdiTb.Text, IPAddress.Parse(ipAdresTb.Text), port, false);
                istemciPencere.Show();
                mw.Close();
            }
            catch(Exception hata) { durum.Text = hata.Message; }
        }
    }
}
