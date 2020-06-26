using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHMobil.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;
using LocalHaberlesmeSinifKutuphanesi.Istemci;
using System.IO;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using LHMobil.Interface;
using System.Diagnostics;

namespace LHMobil
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Istemci : TabbedPage
    {
        public Dictionary<string, string> ozelMesajlar = new Dictionary<string, string>();
        List<CevrimiciK> cevrimiciKullanicilar = new List<CevrimiciK>();
        List<DosyaBilgi> dosyalar = new List<DosyaBilgi>();
        OzelSohbet ozelSohbetSayfa;
        private string _kullaniciAdi = null;
        private string dosyaKayitYol = null;
        private string aktifOzelSohbet = null;

        public Istemci()
        {
            InitializeComponent();
            
        }

        public Istemci(string kullaniciAdi,string kayitYol)
        {
            InitializeComponent();
            _kullaniciAdi = kullaniciAdi;
            dosyaKayitYol = kayitYol;

            ozelSohbetSayfa = new OzelSohbet(this);
            SekmeliSayfa.CurrentPage = SohbetSekme;
            aktifList.ItemsSource = cevrimiciKullanicilar;
            if (IstemciSinifi.istemciDurum)
            {
                Thread th = new Thread(new ThreadStart(MesajAl));
                th.IsBackground = true;
                th.Start();
            }
        }

        public void MesajAl()
        {
            while (IstemciSinifi.istemciDurum)
            {
                string mesaj = IstemciSinifi.MesajAl();
                if (mesaj != null && mesaj != "BK")
                    Yazdir(mesaj);
            }
        }

        public void Yazdir(string mesaj)
        {
            string[] ozelMesajKontrol = mesaj.Split(':');
            if (ozelMesajKontrol[0].Contains("§") && ozelMesajKontrol[0].Contains("§OzelMesaj§"))
            {
                Device.BeginInvokeOnMainThread(() =>
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
                    string yazdirilacakOzelMesaj = DateTime.Now.ToShortTimeString() + " - " + gonderen + ":" + ozelmesaj + "\n";
                    ozelMesajlar[gonderen] += yazdirilacakOzelMesaj;
                    if (aktifOzelSohbet == gonderen)
                    {
                        ozelSohbetSayfa.SohbeteYazdir(yazdirilacakOzelMesaj);
                    }
                    foreach (CevrimiciK kul in cevrimiciKullanicilar)
                    {
                        if (kul.KullaniciAdi == gonderen)
                        {
                            kul.BildirimSayi++;
                            AktifListGuncelle();
                            break;
                        }
                    }
                });
            }
            else if (mesaj.Contains("§") && !mesaj.Contains(":"))
            {
                string[] bolunmus = mesaj.Split('§');
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (bolunmus[1] == "KullaniciBaglandi")
                    {
                        Sohbet.Text = Sohbet.Text + DateTime.Now.ToShortTimeString() + " - " + bolunmus[0] + " Bağlandı.\n";
                        cevrimiciKullanicilar.Add(new CevrimiciK { KullaniciAdi = bolunmus[0], BildirimSayi = 0 });
                        AktifListGuncelle();
                        ozelMesajlar.Add(bolunmus[0], null);
                    }
                    else if (bolunmus[1] == "Baglandi")
                    {
                        cevrimiciKullanicilar.Add(new CevrimiciK { KullaniciAdi = bolunmus[0],BildirimSayi=0 });
                        ozelMesajlar.Add(bolunmus[0], null);
                    }
                    else if (bolunmus[1] == "KullaniciAyrildi")
                    {
                        Sohbet.Text = Sohbet.Text + DateTime.Now.ToShortTimeString() + " - " + bolunmus[0] + " Ayrıldı.\n";
                        foreach (CevrimiciK kullanici in cevrimiciKullanicilar)
                        {
                            if (kullanici.KullaniciAdi == bolunmus[0])
                            {
                                cevrimiciKullanicilar.Remove(kullanici);
                                break;
                            }
                        }  // listeden sil
                        AktifListGuncelle();
                        ozelMesajlar.Remove(bolunmus[0]);
                    }
                    else if (bolunmus[1] == "DosyaTransferBilgi")
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Sohbet.Text = Sohbet.Text + DateTime.Now.ToShortTimeString() + " - " + bolunmus[0] + " bir dosya gönderdi.\n";
                            dosyalar.Add(new DosyaBilgi { Saat = DateTime.Now.ToShortTimeString(), Kod = bolunmus[3], Gonderen = bolunmus[0], Dosya = bolunmus[2], Boyut = bolunmus[4] });
                            dosyaTransfer.ItemsSource = null;
                            dosyaTransfer.ItemsSource = dosyalar;
                        });
                    }
                    else if (bolunmus[1] == "DosyaIstekCevap")
                    {
                        string data = bolunmus[2];
                        foreach (DosyaBilgi dosyaBilgi in istekDosyalar)
                        {
                            if (dosyaBilgi.Kod == bolunmus[0])
                            {
                                File.WriteAllBytes(dosyaKayitYol + "/" + dosyaBilgi.Dosya, Convert.FromBase64String(data));
                            }
                            istekDosyalar.Remove(dosyaBilgi);
                            break;
                        }
                    }
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Sohbet.Text = Sohbet.Text + DateTime.Now.ToShortTimeString() + " - " + mesaj + "\n";
                    if (mesaj == "Sunucu Kapatıldı.")
                    {
                        cevrimiciKullanicilar.Clear();
                        AktifListGuncelle();
                    }
                });

            }
        }

        private void AktifListGuncelle()
        {
            aktifList.ItemsSource = null;
            aktifList.ItemsSource = cevrimiciKullanicilar;
        }

        private void GonderBtn_Clicked(object sender, EventArgs e)
        {
            MesajGonder();
            
        }

        public void MesajGonder()
        {
            if (IstemciSinifi.MesajGonderebilmeDurum)
            {
                if (Mesaj.Text != "")
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
                        IstemciSinifi.MesajGonder(Mesaj.Text);
                        Mesaj.Text = null;
                    }
                }
            }
        }

        public List<DosyaBilgi> istekDosyalar = new List<DosyaBilgi>();
        async private void DosyaTransfer_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            DosyaBilgi secilenDosya = (DosyaBilgi)dosyaTransfer.SelectedItem;
            bool kabul = await DisplayAlert("Dosya indirilsin mi?", $"Saat:{secilenDosya.Saat}\nDosya Kod:{secilenDosya.Kod}\nGönderen:{secilenDosya.Gonderen}\nDosya:{secilenDosya.Dosya}\nBoyut:{secilenDosya.Boyut}", "İndir", "Vazgeç");
            if (kabul)
            {
                try
                {
                    istekDosyalar.Add(secilenDosya);
                    IstemciSinifi.DosyaIstek(secilenDosya.Kod);
                }
                catch(Exception exc) { await DisplayAlert("Hata", exc.Message, "Tamam"); }
            }
        }

        private void AktifList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                CevrimiciK secilenCevrimici = (CevrimiciK)aktifList.SelectedItem;
                if (secilenCevrimici.KullaniciAdi != _kullaniciAdi)
                {
                    ozelSohbetSayfa.Yenile(secilenCevrimici.KullaniciAdi, ozelMesajlar[secilenCevrimici.KullaniciAdi]);
                    aktifOzelSohbet = secilenCevrimici.KullaniciAdi;
                    foreach (CevrimiciK kul in cevrimiciKullanicilar)
                    {
                        if (kul.KullaniciAdi == secilenCevrimici.KullaniciAdi)
                        {
                            kul.BildirimSayi = 0;
                        }
                        AktifListGuncelle();
                        break;
                    }
                    Navigation.PushAsync(ozelSohbetSayfa);
                }
            }
            catch (Exception ex) { DisplayAlert("Hata", ex.Message, "Tamam"); }
        }

        async private void DosyaGonderBtn_Clicked(object sender, EventArgs e)
        {
            try
            { 
                FileData dosya = await CrossFilePicker.Current.PickFile();
                string dosyaBoyutStr = null;

                #region Dosya Boyut Bulma
                FileStream fs = new FileStream(dosya.FilePath, FileMode.Open, FileAccess.Read);
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

                bool kabul = await DisplayAlert("Dosya gönderilsin mi?", $"{dosya.FileName}({dosyaBoyutStr}) isimli dosyayı göndermek istediğinize emin misiniz?", "Gönder", "Vazgeç");
                if (kabul)
                {
                    IstemciSinifi.DosyaGonder(dosya.FileName, dosya.FilePath, dosyaBoyutStr);
                }
            }
            catch(Exception ex) { await DisplayAlert("Hata",ex.Message,"Tamam"); }
        }


        private void IndirilenlerAc_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("İndirilenler Klasörü","Kayıt Yolu: "+dosyaKayitYol, "Tamam");
        }
    }       
}           