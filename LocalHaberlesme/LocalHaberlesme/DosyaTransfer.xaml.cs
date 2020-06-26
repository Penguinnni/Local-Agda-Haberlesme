using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LocalHaberlesmeSinifKutuphanesi.Istemci;
using LocalHaberlesmeSinifKutuphanesi.Sunucu;
using System.IO;

namespace LocalHaberlesme
{
    /// <summary>
    /// DosyaTransfer.xaml etkileşim mantığı
    /// </summary>
    public partial class DosyaTransfer : Page
    {
        private Popup a;
        public List<Veri> istekListesi = new List<Veri>();
        private bool host = false;
        public string kayitYol = null;
        public DosyaTransfer(Popup popup,bool host,string kayitYol)
        {
            InitializeComponent();
            a = popup;
            this.host = host;
            this.kayitYol = kayitYol;
        }

        public void VeriEkle(string kod,string gonderen,string dosyaAd,string dosyaBoyut)
        {
            dg.Items.Add(new Veri {Saat=DateTime.Now.ToShortTimeString(), Kod=kod,Gonderen=gonderen,Dosya=dosyaAd,Boyut=dosyaBoyut});
        }


        private void Kapat_Click(object sender, RoutedEventArgs e)
        {
            a.IsOpen = false;
        }

        private void GonderBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dia = new OpenFileDialog();
            bool d = (bool)dia.ShowDialog();
            if (d)
            {
                string dosyaBoyutStr = null;

                #region Dosya Boyut Bulma
                FileStream fs = new FileStream(dia.FileName, FileMode.Open, FileAccess.Read);
                double dosyaBoyut = fs.Length;
                fs.Close();
                string[] boyutlar = { "byte", "KB", "MB", "GB" };
                int sayac = 0;
                while (dosyaBoyut >= 1024 && sayac <= 3)
                {
                    dosyaBoyut = dosyaBoyut / 1024;
                    sayac++;
                }
                dosyaBoyutStr = Math.Round(dosyaBoyut, 2) + " " + boyutlar[sayac];
                #endregion

                if (MessageBox.Show($"Dosya: {dia.SafeFileName}\nBoyut: {dosyaBoyutStr}\ngönderilsin mi?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    IstemciSinifi.DosyaGonder(dia.SafeFileName, dia.FileName,dosyaBoyutStr);
                }
            }
        }

        private void DosyaIste_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Veri satir = (Veri)dg.SelectedItem;
                if (MessageBox.Show($"Dosya: {satir.Dosya}\nBoyut: {satir.Boyut}\nindirilsin mi?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (!host)
                    {
                        istekListesi.Add(satir);
                        IstemciSinifi.DosyaIstek(satir.Kod);
                    }
                    else
                    {
                        File.WriteAllBytes(kayitYol + "\\" + satir.Dosya, Convert.FromBase64String(SunucuSinifi.DosyaAl(satir.Kod)));
                    }
                }
            }
            catch { MessageBox.Show("Dosya Seçilmedi", "Hata"); }
        }

        private void KlasorAc_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Directory.GetCurrentDirectory()+"\\İndirilenler");
        }
    }

    public class Veri
    {
        public string Saat { get; set; }
        public string Kod { get; set; }
        public string Gonderen { get; set; }
        public string Dosya { get; set; }
        public string Boyut { get; set; }


    }
}
