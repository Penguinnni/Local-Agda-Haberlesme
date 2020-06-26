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
using LocalHaberlesmeSinifKutuphanesi.Istemci;
using LocalHaberlesmeSinifKutuphanesi.Sunucu;
using System.Net;
using System.Threading;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;

namespace LocalHaberlesme
{
    /// <summary>
    /// Istemci.xaml etkileşim mantığı
    /// </summary>
    public partial class Istemci : Window
    {
        public string kullaniciAdi;
        public string kayitYol=null;
        public IPAddress ipAdres;
        public int port;
        public bool host;
        public List<CevrimiciK> cevrimiciKullanicilar = new List<CevrimiciK>();
        public OzelSohbet os;
        public Dictionary<string, string> ozelMesajlar = new Dictionary<string, string>();

        public List<string> yaziyorList = new List<string>();
        public string aktifOzelMesaj = null;
        DosyaTransfer dosyaTransfer;
        

        public Istemci(string kullaniciAdi, IPAddress ipAdres, int port, bool host)
        {
            InitializeComponent();
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\İndirilenler");
            kayitYol = Directory.GetCurrentDirectory() + "\\İndirilenler";
            kayitYolTb.Text = "Dosya Kayıt Yolu: " + kayitYol;
         
            #region Gelen değerleri aktarma
            this.kullaniciAdi = kullaniciAdi;
            this.ipAdres = ipAdres;
            this.port = port;
            this.host = host;
            #endregion

            #region Arayüz ayarları
            baslik.Content = kullaniciAdi+ " - " + ( host ? "Sunucu":"Istemci");
            ipAdressLbl.Content = ipAdressLbl.Content + ipAdres.ToString();
            portLbl.Content = portLbl.Content + port.ToString();
            if(host) Sohbet.Text="Sohbet Oluşturuldu.\n";
            baglantiKes.Content = host ? "Sunucuyu Kapat" : "Bağlantıyı Kes";
            if (host) this.Title = "Sunucu";
            #endregion

            #region İstemci
                Thread MesajAlici = new Thread(new ThreadStart(MesajAl));
                MesajAlici.IsBackground = true;
                MesajAlici.Start();
            #endregion

            dosyaTransfer = new DosyaTransfer(OzelSohbetPopup, host,kayitYol);
            os = new OzelSohbet(OzelSohbetPopup,this);
        }

        public void MesajAl()
        {
            while (IstemciSinifi.istemciDurum)
            {
                string mesaj = IstemciSinifi.MesajAl();
                if(mesaj != null)
                    Yazdir(mesaj);
            }
        }

        public void BildirimArttir(string kullaniciAd)
        {
                foreach (CevrimiciK ck in cevrimiciKullanicilar)
                {
                    if (ck.KullaniciAdi == kullaniciAd)
                    {
                        ck.BildirimSayisi++;
                        break;
                    }
                }
                CevrimiciGuncelle();
            }

        public void OzelMesajGonder(string mesaj)
        {
            ozelMesajlar[aktifOzelMesaj] += DateTime.Now.ToShortTimeString() + " - " + kullaniciAdi + ": " + mesaj + "\n";
            os.EkranaYazdir(kullaniciAdi + ": " + mesaj + "\n");
            IstemciSinifi.OzelMesaj(aktifOzelMesaj, mesaj);
        }

        public void Yazdir(string mesaj)
        {
            if (mesaj != "BK")
            {
                string[] ozelMesajKontrol = mesaj.Split(':');
                if (ozelMesajKontrol[0].Contains("§") && ozelMesajKontrol[0].Contains("§OzelMesaj§"))
                {
                    string[] ozelMesajKontrol2 = ozelMesajKontrol[0].Split('§');
                    string gonderen = ozelMesajKontrol2[2];
                    string ozelmesaj = ozelMesajKontrol[1];

                    if (ozelMesajKontrol.Length > 2)
                    {
                        for (int i = 2; i < ozelMesajKontrol.Length; i++)
                        {
                            ozelmesaj = ozelmesaj + ":" + ozelMesajKontrol[i];
                        }
                    }

                    string yazdirilacakOzelMesaj = gonderen + ":" + ozelmesaj + "\n";
                    ozelMesajlar[gonderen] += DateTime.Now.ToShortTimeString() + " - " + yazdirilacakOzelMesaj;

                    Dispatcher.Invoke(() =>
                    {
                        if (OzelSohbetPopup.IsOpen == false)
                        {
                            BildirimArttir(gonderen);
                        }
                        else
                        {
                            if (gonderen != aktifOzelMesaj)
                            {
                                BildirimArttir(gonderen);
                            }
                            else
                                os.EkranaYazdir(yazdirilacakOzelMesaj);
                        }
                    });
                }
                else if (mesaj.Contains("§") && !mesaj.Contains(":"))
                {
                    string[] bolunmus = mesaj.Split('§');
                    this.Dispatcher.Invoke(() =>
                    {
                        if (bolunmus[1] == "KullaniciBaglandi")
                        {
                            cevrimiciKullanicilar.Add(new CevrimiciK { KullaniciAdi = bolunmus[0], BildirimSayisi = 0 });
                            CevrimiciGuncelle();
                            ozelMesajlar.Add(bolunmus[0], "");
                            Sohbet.Text = Sohbet.Text + DateTime.Now.ToShortTimeString() + " - " + bolunmus[0] + " Bağlandı.\n";
                        }
                        else if (bolunmus[1] == "Baglandi")
                        {
                            cevrimiciKullanicilar.Add(new CevrimiciK { KullaniciAdi = bolunmus[0], BildirimSayisi = 0 });
                            CevrimiciGuncelle();
                            ozelMesajlar.Add(bolunmus[0], "");

                        }
                        else if (bolunmus[1] == "KullaniciAyrildi")
                        {
                            foreach (CevrimiciK ayrilanKullanici in cevrimiciKullanicilar)
                            {
                                if (bolunmus[0] == ayrilanKullanici.KullaniciAdi)
                                {
                                    cevrimiciKullanicilar.Remove(ayrilanKullanici);
                                    break;
                                }
                            }
                            ozelMesajlar.Remove(bolunmus[0]);
                            CevrimiciGuncelle();
                            Sohbet.Text = Sohbet.Text + DateTime.Now.ToShortTimeString() + " - " + bolunmus[0] + " Ayrıldı.\n";

                        }
                        else if (bolunmus[1] == "DosyaTransferBilgi")
                        {
                            Dispatcher.Invoke(() =>
                            {
                                Sohbet.Text = Sohbet.Text + DateTime.Now.ToShortTimeString() + " - " + bolunmus[0] + " bir dosya gönderdi.\n";
                                dosyaTransfer.VeriEkle(bolunmus[3], bolunmus[0], bolunmus[2], bolunmus[4]);
                            });
                        }
                        else if (bolunmus[1] == "DosyaIstekCevap")
                        {
                            string data = bolunmus[2];
                            foreach (Veri dosyaBilgi in dosyaTransfer.istekListesi)
                            {
                                if (dosyaBilgi.Kod == bolunmus[0])
                                {
                                    File.WriteAllBytes(kayitYol + "\\" + dosyaBilgi.Dosya, Convert.FromBase64String(data));
                                }
                                dosyaTransfer.istekListesi.Remove(dosyaBilgi);
                                break;
                            }
                        }
                    });
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {

                        Sohbet.Text = Sohbet.Text + DateTime.Now.ToShortTimeString() + " - " + mesaj + "\n";
                        if (mesaj == "Sunucu Kapatıldı.")
                        {
                            cevrimiciKullanicilar.Clear();
                            CevrimiciGuncelle();
                            ozelMesajlar.Clear();
                        }
                    });
                }
            }
        }
        
        public void MesajGonder()
        {
            bool kontrol = false;
            if (IstemciSinifi.MesajGonderebilmeDurum)
            {
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
                    IstemciSinifi.MesajGonder(Mesaj.Text);
                    Mesaj.Text = null;
                }
            }
        }

        private void Mesaj_KeyDown(object sender, KeyEventArgs e)
        {
                if (Key.Enter == e.Key)
                {
                    MesajGonder();
                }
        }

        private void Gonderbuton_Click(object sender, RoutedEventArgs e)
        {
            MesajGonder();
        }

        private void Temizlebuton_Click(object sender, RoutedEventArgs e)
        {
            Sohbet.Text = null;
        }

        private void Ustcubuk_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void KucultBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Kapatbtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (host)
            {
                IstemciSinifi.BaglantiKes();
                SunucuSinifi.SunucuDurdur();
            }
            else
                IstemciSinifi.BaglantiKes();
        }

        private void BaglantiKes_Click(object sender, RoutedEventArgs e)
        {
            if (host)
            {
                IstemciSinifi.BaglantiKes();
                SunucuSinifi.SunucuDurdur();
            }
            else
                IstemciSinifi.BaglantiKes();

            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        public void CevrimiciGuncelle()
        {
            aktiflist.ItemsSource = null;
            aktiflist.ItemsSource = cevrimiciKullanicilar;
        }

        private void Aktiflist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(aktiflist.SelectedIndex != -1)
            {
                CevrimiciK kullanici = (CevrimiciK)aktiflist.SelectedItem;
                if (kullanici.KullaniciAdi != kullaniciAdi)
                {
                    os.OzelSohbetYenile(kullanici.KullaniciAdi, ozelMesajlar[kullanici.KullaniciAdi]);
                    pencere.Navigate(os);
                    OzelSohbetPopup.IsOpen = true;
                    aktifOzelMesaj = kullanici.KullaniciAdi;
                    foreach (CevrimiciK c in cevrimiciKullanicilar)
                    {
                        if (c.KullaniciAdi == kullanici.KullaniciAdi)
                        {
                            c.BildirimSayisi = 0;
                            break;
                        }
                    }
                    CevrimiciGuncelle();
                }
            }

        }

        private void Mesaj_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Sohbet_TextChanged(object sender, TextChangedEventArgs e)
        {
            Sohbet.ScrollToEnd();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            OzelSohbetPopup.IsOpen = false;
        }

        private void OzelSohbetPopup_Closed(object sender, EventArgs e)
        {
            aktifOzelMesaj = null;
        }

        private void KayitYerDegistirBtn_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog klasorSec = new CommonOpenFileDialog();
            klasorSec.Title = "Dosya Kayıt Klasörü Seçiniz";
            klasorSec.IsFolderPicker = true;
            klasorSec.AllowNonFileSystemItems = false;
            klasorSec.Multiselect = false;
            klasorSec.ShowPlacesList = true;
            if (klasorSec.ShowDialog() == CommonFileDialogResult.Ok)
            {
                kayitYol = klasorSec.FileName;
                dosyaTransfer.kayitYol = klasorSec.FileName;
                kayitYolTb.Text = "Dosya Kayıt Yolu: " + klasorSec.FileName;
            }
        }

        private void EkBtn_Click(object sender, RoutedEventArgs e)
        {
            pencere.Navigate(dosyaTransfer);
            OzelSohbetPopup.IsOpen = true;
        }
    }

    public class CevrimiciK
    {
        public string KullaniciAdi { get; set; }
        public int BildirimSayisi { get; set; }

        public string Bildirim
        {
            get
            {
                if (BildirimSayisi>0)
                    return "🔔"+BildirimSayisi;
                else
                    return null;
            }
        }
    }
}
