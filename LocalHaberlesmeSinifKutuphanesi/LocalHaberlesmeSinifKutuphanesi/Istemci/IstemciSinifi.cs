using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace LocalHaberlesmeSinifKutuphanesi.Istemci
{
    public static class IstemciSinifi
    {
        public static bool istemciDurum = false;
        public static bool MesajGonderebilmeDurum = true;
        private static TcpClient istemciSoketi;
        private static NetworkStream akis;
        private static BinaryReader oku;
        private static BinaryWriter yaz;
        private static IPAddress ip;
        private static int port;
        public static string kullaniciAdi = null;

        public static void Baglan(string _kullaniciAdi, IPAddress _ip, int _port) // Sunucuya bağlanır ve mesjlaşmada kullanılacak sınıfları oluşturur.
        {
            istemciSoketi = new TcpClient();
            try
            {
                istemciSoketi.Connect(_ip, _port);
                istemciDurum = true;
            }
            catch { throw new Exception("Sunucuya Bağlanılamadı"); }

            kullaniciAdi = _kullaniciAdi;
            ip = _ip;
            port = _port;

            akis = istemciSoketi.GetStream();
            oku = new BinaryReader(akis,Encoding.UTF8);
            yaz = new BinaryWriter(akis,Encoding.UTF8);

            yaz.Write("KullaniciAdiKontrol");

            bool kontrol = false;
            while (true)
            {
                string kullanici = oku.ReadString();
                if (kullanici == "§Tamamlandi§")
                    break;
                if (kullanici == kullaniciAdi)
                {
                    kontrol = true;
                    break;
                }
            }
            if (kontrol)
            {
                yaz.Write("UygunOlmayanKullaniciAdi");
                istemciDurum = false;
                throw new Exception("Kullanıcı adı kullanıldığı için bağlanılamadı.");
            }


            if (istemciDurum)
            {
                yaz.Write(kullaniciAdi + "§KullaniciBaglandi");
            }
        }

        public static void BaglantiKes()
        {
            if (istemciDurum)
            {
                yaz.Write(kullaniciAdi + "§KullaniciAyrildi");
                istemciDurum = false;
                istemciSoketi.Close();
                
            }
        }

        public static void MesajGonder(string mesaj) 
        {
            if (istemciDurum)
            {
                if (istemciSoketi.Connected)
                {
                    if (MesajGonderebilmeDurum)
                    {
                        yaz.Write(kullaniciAdi + ": " + mesaj);
                    }
                }
                else istemciDurum = false;
            }
        }

        public static void DosyaGonder(string dosyaAd,string dosyaYol,string dosyaBoyut)
        {
            if (istemciDurum)
            {
                if (istemciSoketi.Connected)
                {
                    string[] adVeYol = { dosyaAd, dosyaYol, dosyaBoyut };
                    Thread dosyaGonderici = new Thread(new ParameterizedThreadStart(DosyaGonderTh));
                    dosyaGonderici.Start(adVeYol);
                }
                else istemciDurum = false;
            }
        }

        private static void DosyaGonderTh(object obj)
        {
            string[] adVeYol = (string[])obj;
            try
            {
                byte[] dosyaByte = File.ReadAllBytes(adVeYol[1]);
                string dosya = kullaniciAdi + "§DosyaTransfer§" + adVeYol[0] + "§"+ adVeYol[2]+"§";
                dosya = dosya + Convert.ToBase64String(dosyaByte);
                MesajGonderebilmeDurum = false;
                yaz.Write(dosya);
                MesajGonderebilmeDurum = true;
            }
            catch { }
        }

        public static void DosyaIstek(string kod)
        {
            if (istemciDurum)
            {
                if (istemciSoketi.Connected)
                {
                    if (MesajGonderebilmeDurum)
                    {
                        yaz.Write(kod + "§DosyaIstek");
                    }
                }
                else istemciDurum = false;
            }
        }


        public static void OzelMesaj(string aliciKullaniciAdi, string mesaj)
        {
            // ALİCİ§OzelMesaj§GÖNDEREN": "+mesaj
            if (istemciDurum)
            {
                if (istemciSoketi.Connected)
                {
                    if (MesajGonderebilmeDurum)
                    {
                        yaz.Write(aliciKullaniciAdi + "§OzelMesaj§" + kullaniciAdi + ": " + mesaj);
                    }
                }
                else istemciDurum = false;
            }
        }

        public static string MesajAl()
        {
            if (istemciDurum)
            {
                if (istemciSoketi.Connected)
                {
                    string mesaj = null;
                    try { mesaj = oku.ReadString(); } catch {}
                    if (mesaj == "Sunucu Kapatıldı.")
                        istemciDurum = false;
                    return mesaj;
                }
                else
                {
                    istemciDurum = false;
                    return null;
                }
            }
            else return null;
        }
    }
}
