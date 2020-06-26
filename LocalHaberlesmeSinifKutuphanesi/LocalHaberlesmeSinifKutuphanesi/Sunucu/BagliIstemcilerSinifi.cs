using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace LocalHaberlesmeSinifKutuphanesi.Sunucu
{
    internal class BagliIstemcilerSinifi
    {
        public string kullaniciAdi = null;
        public TcpClient istemciSoket;
        public NetworkStream akis;
        public BinaryReader oku;
        public BinaryWriter yaz;
        public bool istemciDurum = false;
        public bool kontrol = false;
        private Thread MesajAlici;
        private Thread BaglantiKontrolTh;
        public bool MesajGonderebilmeDurum = true;

        private List<string> beklemedekiMesajlar;

        public BagliIstemcilerSinifi(TcpClient istemciSoket) //Sınıf örneklenirken bağlanan istemciyi alır
        {
            this.istemciSoket = istemciSoket;
        }


        public void Calistir() // Mesaj alıp vermede kullanılan sınıfları oluşturur ve bağlı istemciden gelecek mesajları dinlemeye başlar
        {
            if (istemciSoket.Connected)
            {
                istemciDurum = true;
                akis = istemciSoket.GetStream();
                beklemedekiMesajlar = new List<string>();
                oku = new BinaryReader(akis,Encoding.UTF8);
                yaz = new BinaryWriter(akis,Encoding.UTF8);
                MesajAlici = new Thread(new ThreadStart(MesajAl));
                MesajAlici.IsBackground = true;
                MesajAlici.Start();

                BaglantiKontrolTh = new Thread(new ThreadStart(BaglantiKontrol));
                BaglantiKontrolTh.IsBackground = true;
            }
        }

        public void MesajGonder(string mesaj)// Bağlı istemciye mesaj gönderir
        {
            if (MesajGonderebilmeDurum)
            {
                if (kullaniciAdi != null)
                {
                    if (istemciDurum)
                    {
                        if (istemciSoket.Connected)
                        {
                            yaz.Write(mesaj);
                        }
                    }
                }
            }
            else
            {
                beklemedekiMesajlar.Add(mesaj);
            }
        }

        public void DosyaGonder(string dosya)// Bağlı istemciye mesaj gönderir
        {
            if (MesajGonderebilmeDurum)
            {
                if (kullaniciAdi != null)
                {
                    if (istemciDurum)
                    {
                        if (istemciSoket.Connected)
                        {
                            MesajGonderebilmeDurum = false;
                            yaz.Write(dosya);
                            MesajGonderebilmeDurum = true;
                            foreach(string msj in beklemedekiMesajlar)
                            {
                                MesajGonder(msj);
                            }

                            beklemedekiMesajlar.Clear();
                        }
                    }
                }
            }
        }

        public void BaglantiKontrol()
        {
            while (istemciDurum)
            {
                Thread.Sleep(3000);
                if (kullaniciAdi != null)
                {
                    try
                    {
                        if (MesajGonderebilmeDurum)
                        {
                            yaz.Write("BK");
                        }
                    }
                    catch (Exception hata)
                    {
                        // -2146232800 = Bağlantı olmadığı zaman gelen hata kodu
                        if (hata.HResult == -2146232800)
                        {
                            istemciDurum = false;
                            SunucuSinifi.MesajDagit(kullaniciAdi + "§KullaniciAyrildi");
                            SunucuSinifi.SoketSil(this.GetHashCode());
                        }
                    }
                }
            }
        }

        private void MesajAl() //Bağlı olan istemciden mesajı gerekli işlem varsa yapıp sunucuya bağlı olan bütün istemcilere gönderir
        {
            while (istemciSoket.Connected)
            {
                if (istemciDurum)
                {
                    try
                    {
                        string mesaj = oku.ReadString(); // istemciden gelen mesajı alır

                        if (mesaj != null)
                        {
                            string[] ozelMesajKontrol = mesaj.Split(':');
                            if (ozelMesajKontrol[0].Contains("§") && ozelMesajKontrol[0].Contains("§OzelMesaj§"))
                            {
                                string[] ozelMesajKontrol2 = ozelMesajKontrol[0].Split('§');
                                string alici = ozelMesajKontrol2[0];

                                SunucuSinifi.OzelMesajGonder(alici, mesaj);
                                continue;
                            }
                            else if (mesaj.Contains("§") && !(mesaj.Contains(":"))) // Mesaj ile bir işlem yapılıp yapılmayacağını kontrol eder
                            {
                                string[] mesajKontrol = mesaj.Split('§');
                                if (mesajKontrol[1] == "KullaniciBaglandi")
                                {
                                    kullaniciAdi = mesajKontrol[0];
                                    bool kont = SunucuSinifi.BagliIstemcileriGonder(this.yaz,kullaniciAdi); // bağlı istemcileri bağlanan istemciye gönderir
                                    if (kont) BaglantiKontrolTh.Start();
                                }
                                else if (mesajKontrol[1] == "KullaniciAyrildi")
                                {
                                        istemciDurum = false;
                                        SunucuSinifi.SoketSil(this.GetHashCode());
                                }
                                else if (mesajKontrol[1] == "DosyaTransfer")
                                {
                                    int i = SunucuSinifi.NotDefterKayit(mesajKontrol[4]);
                                    SunucuSinifi.MesajDagit(kullaniciAdi+ "§DosyaTransferBilgi§"+mesajKontrol[2]+ "§" + i + "§" +mesajKontrol[3]);
                                    continue;
                                }
                                else if (mesajKontrol[1] == "DosyaIstek")
                                {
                                    string dosya = SunucuSinifi.DosyaAl(mesajKontrol[0]);
                                    DosyaGonder(mesajKontrol[0]+ "§DosyaIstekCevap§" + dosya);
                                    continue;
                                }
                            }
                            if (mesaj == "KullaniciAdiKontrol") { SunucuSinifi.KullaniciAdiKontrol(this.yaz); continue; }
                            else if (mesaj == "UygunOlmayanKullaniciAdi")
                            {
                                istemciDurum = false;
                                SunucuSinifi.SoketSil(this.GetHashCode());
                                continue;
                            }

                            SunucuSinifi.MesajDagit(mesaj); // Gelen mesaj tüm istemcilere gönderilir.
                        }
                    }
                    catch { continue; }
                }
                else break;
            }
        }
    }
}



