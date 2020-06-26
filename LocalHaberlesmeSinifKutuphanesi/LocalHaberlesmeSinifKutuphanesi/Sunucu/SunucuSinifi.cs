using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Timers;

namespace LocalHaberlesmeSinifKutuphanesi.Sunucu
{

    public static class SunucuSinifi
    {
        private static bool sunucuDurum = false;
        private static TcpListener sunucuSoket;
        public static string kayitYol = null;
        private static List<BagliIstemcilerSinifi> bagliIstemciListesi;
        public static Thread IstemciKabulEdici;

        public static void SunucuBaslat(int port,string sunucuDosyaKayitYol)// Sunucuyu aktif eder ve gelen bağlantı isteklerini kabul eden metodu çalıştırır.
        {
            kayitYol = sunucuDosyaKayitYol;
            bagliIstemciListesi = new List<BagliIstemcilerSinifi>();

            try
            {
                sunucuSoket = new TcpListener(IPAddress.Any, port);
                sunucuSoket.Start();
                sunucuDurum = true;
            }
            catch { throw new Exception("Sunucu Başlatılamadı."); }
            IstemciKabulEdici = new Thread(new ThreadStart(IstemciKabul));
            IstemciKabulEdici.IsBackground = true;
            IstemciKabulEdici.Start();
        }

        public static bool PortKontrol(int port)//İstenilen portun kullanılıp kullanılmadığını kontrol edip true yada false gönderir
        {
            try
            {
                Socket soket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPHostEntry hostEntry = Dns.GetHostByName(Dns.GetHostName());
                IPAddress ipAdres = hostEntry.AddressList[hostEntry.AddressList.Length - 1];
                soket.Connect(ipAdres, port);
                return true;
            }
            catch { return false; }
        }

        public static void SunucuDurdur()// Sunucunun durduğunu bağlı istemcilere ilettikten sonra sunucu kapanır.
        {
            foreach(BagliIstemcilerSinifi istemci in bagliIstemciListesi)
            {
                istemci.MesajGonder("Sunucu Kapatıldı.");
                istemci.istemciDurum = false;
            }
            bagliIstemciListesi.Clear();
            sunucuSoket.Stop();
            IstemciKabulEdici.Abort();
            sunucuDurum = false;

            DirectoryInfo klasor = new DirectoryInfo(kayitYol);
            foreach (FileInfo dosya in klasor.GetFiles())
            {
                dosya.Delete();
            }
        }

        public static bool BagliIstemcileriGonder(BinaryWriter yaz,string kendiKAd) // Yeni bağlanan bir istemciye çevrimiçi olan istemcileri gönderir.
        {
            if (sunucuDurum)
            {
                foreach (BagliIstemcilerSinifi istemci in bagliIstemciListesi)
                {
                    if (istemci.istemciDurum)
                    {
                        if (istemci.kullaniciAdi != kendiKAd)
                        {
                            yaz.Write(istemci.kullaniciAdi.ToString() + "§Baglandi");
                        }
                    }
                }
                return true;
            }
            else return false;
        }

        public static void SoketSil(int hasCode) // Verilen hascode ye sahip baglı istemciyi listeden siler
        {
            foreach(BagliIstemcilerSinifi ist in bagliIstemciListesi)
            {
                if (ist.GetHashCode() == hasCode)
                {
                    bagliIstemciListesi.Remove(ist);
                    break;
                }
            }
        }

        public static void KullaniciAdiKontrol(BinaryWriter yaz) // Bağlanan yeni istemciye kullanıcı adını kontrol etmesi için bağlı kullanıcıların isimlerini gönderir.
        {
            if (sunucuDurum)
            {
                foreach (BagliIstemcilerSinifi istemci in bagliIstemciListesi)
                {
                    if (istemci.istemciDurum)
                    {
                        if (istemci.kullaniciAdi != null)
                        {
                            yaz.Write(istemci.kullaniciAdi.ToString());
                        }
                    }
                }
                yaz.Write("§Tamamlandi§");
            }
        }

        public static void MesajDagit(string mesaj) // İstemciden gelen mesajı bağlı olan istemcilere gönderir
        {
            if (sunucuDurum)
            {
                foreach (BagliIstemcilerSinifi istemci in bagliIstemciListesi)
                {
                    if (istemci.istemciDurum)
                    {
                        if (istemci.istemciSoket.Connected)
                        {
                            istemci.MesajGonder(mesaj);
                        }
                    }
                }
            }
        }

        private static int kod = 1;
        public static int NotDefterKayit(string dosya) // Istemcinin gönderdiği dosyayı sunucu kayıt klasörüne dosya kodu ile txt dosyası olarak kayıt eder.
        {
            File.Delete(kayitYol+"\\" + kod + ".txt"); // Daha önceden o kod ile bir dosya varsa siler yoksa hiçbirşey yapmaz.
            File.AppendAllText(kayitYol+"\\"+kod + ".txt", dosya); 
            kod++; 
            return kod-1;
        }

        public static string DosyaAl(string istenilenKod) // İstenilen dosyanın kodu ile sunucu kayıt klasöründen dosyayı bulur ve dosyayı döndürür.
        {
            StreamReader sr = new StreamReader(kayitYol+"\\" + istenilenKod + ".txt"); 
            string data = sr.ReadToEnd();
            sr.Close();
            return data;
        }

        public static void OzelMesajGonder(string alici, string mesaj) // İstemciden gelen özel mesajı alıcıya gönderir.
        {
            if (sunucuDurum)
            {
                foreach (BagliIstemcilerSinifi istemci in bagliIstemciListesi) // Bağlı istemcilerde alıcıyı arar.
                {
                    if (istemci.istemciDurum)
                    {
                        if (istemci.kullaniciAdi == alici) // alıcı bulunmuşsa mesajı gönderir.
                        {
                            istemci.MesajGonder(mesaj);
                            break; // Alıcı bulunduğu için arama işlemini bitirir.
                        }
                    }
                }
            }
        }

        private static void IstemciKabul() // İstemci bağlantı isteklerini kabul eder ve listeye ekler.
        {
            while (sunucuDurum)
            {
                try
                {
                    TcpClient istemci = sunucuSoket.AcceptTcpClient();
                    BagliIstemcilerSinifi istemciSinif = new BagliIstemcilerSinifi(istemci);
                    istemciSinif.Calistir();
                    if (istemciSinif.istemciDurum)
                    {
                        bagliIstemciListesi.Add(istemciSinif);
                    }
                }
                catch { continue; }
            }
        }

    }
}