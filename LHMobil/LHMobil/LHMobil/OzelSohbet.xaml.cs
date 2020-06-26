using LocalHaberlesmeSinifKutuphanesi.Istemci;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LHMobil
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OzelSohbet : ContentPage
	{
        private string kad = null;
        Istemci istemciPencere;
        public OzelSohbet()
        {
            InitializeComponent();
        }

        public OzelSohbet(Istemci istemciPencere)
		{
			InitializeComponent();
            this.istemciPencere= istemciPencere;
        }

        public void Yenile(string _kullaniciAd,string Mesajlar)
        {
            this.kad = _kullaniciAd;
            KullaniciAdi.Text = kad;
            Sohbet.Text = Mesajlar;
        }

        public void SohbeteYazdir(string mesaj)
        {
            Sohbet.Text += mesaj;
        }

        private void GonderBtn_Clicked(object sender, EventArgs e)
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
                    string yazdirilacak = DateTime.Now.ToShortTimeString() + " - " + IstemciSinifi.kullaniciAdi + ": " + Mesaj.Text + "\n";
                    SohbeteYazdir(yazdirilacak);
                    istemciPencere.ozelMesajlar[kad] += yazdirilacak;
                    IstemciSinifi.OzelMesaj(kad, Mesaj.Text);
                    Mesaj.Text = null;
                }
            }
        }
    }
}