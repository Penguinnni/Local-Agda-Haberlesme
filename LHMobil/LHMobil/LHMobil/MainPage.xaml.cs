using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using LocalHaberlesmeSinifKutuphanesi.Istemci;
using LHMobil.Ortak;
using System.Net;
using System.IO;
using LHMobil.Interface;

namespace LHMobil
{
    public partial class MainPage : ContentPage
    {
        private bool btn = false;
        private string yol = null;
        public MainPage()
        {
            InitializeComponent();

            OrtakSinif.kAdi_IP_PORTDoldur(kAdiEntry, ipEntry, portEntry);
            yol = DependencyService.Get<IPathService>().PublicExternalFolder + "/LocalHaberlesme/İndirilenler";
            Directory.CreateDirectory(yol);
            if (IstemciSinifi.istemciDurum)
            {
                ipEntry.IsEnabled = false;
                kAdiEntry.IsEnabled = false;
                portEntry.IsEnabled = false;
                okBtn.Text = "Bağlantıyı Kes";
                shbtDon.IsVisible = true;

                btn = true;
            }
        }


        private void OkBtn_Clicked(object sender, EventArgs e)
        {
            if (!(btn))
            {
                try
                {
                    int port = int.Parse(portEntry.Text);

                    OrtakSinif.KullaniciAdiKontrol(kAdiEntry.Text);
                    OrtakSinif.PortveIpKontrol(ipEntry.Text, port);

                    IstemciSinifi.Baglan(kAdiEntry.Text, IPAddress.Parse(ipEntry.Text), port);
                    DisplayAlert("Başarılı", "Bağlanıldı.", "Tamam");
                    ipEntry.IsEnabled = false;
                    kAdiEntry.IsEnabled = false;
                    portEntry.IsEnabled = false;
                    okBtn.Text = "Bağlantıyı Kes";
                    shbtDon.IsVisible = true;

                    btn = true;
                    Tasiyici.Istemci = new Istemci(kAdiEntry.Text,yol);
                    Navigation.PushAsync(Tasiyici.Istemci);
                }
                catch (Exception hata) { DisplayAlert("Hata", hata.Message, "Tamam"); }
            }
            else
            {
                IstemciSinifi.BaglantiKes();
                Tasiyici.Istemci = null;
                ipEntry.IsEnabled = true;
                kAdiEntry.IsEnabled = true;
                portEntry.IsEnabled = true;
                okBtn.Text = "Sohbete Bağlan";
                shbtDon.IsVisible = false;
                btn = false;
            }
        }

        private void KAdiEntry_Completed(object sender, EventArgs e)
        {
            ipEntry.Focus();
        }

        private void IpEntry_Completed(object sender, EventArgs e)
        {
            portEntry.Focus();
        }

        private void ShbtDon_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(Tasiyici.Istemci);
        }
    }
}
