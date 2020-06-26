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
using System.Threading;
using LocalHaberlesmeSinifKutuphanesi.Sunucu;
using LocalHaberlesmeSinifKutuphanesi.Istemci;
using System.Net;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;


namespace LocalHaberlesme
{
    /// <summary>
    /// SunucuOlustur.xaml etkileşim mantığı
    /// </summary>
    public partial class SunucuOlustur : Page
    {
        Window mw;
        public SunucuOlustur(Window mw)
        {
            InitializeComponent();
            Ortak.OrtakSinif.kAdi_IP_PORTDoldur(kullaniciAdiTb, ipAdresTb, portTb);
            this.mw = mw;
        }

        private void OlusturBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int port = int.Parse(portTb.Text);
                Ortak.OrtakSinif.KullaniciAdiKontrol(kullaniciAdiTb.Text);
                Ortak.OrtakSinif.PortveIpKontrol(ipAdresTb.Text, port);
                string yol = Directory.GetCurrentDirectory() + "\\SunucuKayit";
                Directory.CreateDirectory(yol);

                if (!(SunucuSinifi.PortKontrol(port)))
                {
                    SunucuSinifi.SunucuBaslat(int.Parse(portTb.Text), yol);
                    IstemciSinifi.Baglan(kullaniciAdiTb.Text + "(HOST)", IPAddress.Parse(ipAdresTb.Text), port);
                }
                else throw new Exception("Port kullanıldığı için sunucu oluşturulamadı.");

                Istemci istemciPencere = new Istemci(kullaniciAdiTb.Text + "(HOST)", IPAddress.Parse(ipAdresTb.Text), port, true);
                istemciPencere.Show();
                mw.Close();
            }
            catch (Exception hata) { durum.Text = hata.Message; }
        }
    }
}
