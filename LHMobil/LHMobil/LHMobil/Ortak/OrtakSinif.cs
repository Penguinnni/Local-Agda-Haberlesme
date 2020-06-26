using System;
using System.Net;
using System.Linq;
using Xamarin.Forms;

namespace LHMobil.Ortak
{
    public static class OrtakSinif
    {

        static int[] trKrkAscii = { 231, 305, 287, 246, 351, 252, 199, 304, 208, 214, 350, 220 };
        public static void KullaniciAdiKontrol(string kullaniciAdi)
        {
            if (kullaniciAdi == "" || kullaniciAdi.Contains(" "))
                throw new Exception("Kullanıcı adı içerisinde boşluk karakteri olamaz ve boş bırakılamaz.");
            if (!(kullaniciAdi.Length >= 6 && kullaniciAdi.Length <= 12))
                throw new Exception("Kullanıcı adı 6-12 karakterden oluşmalıdır.");
            char[] kAdKarakterleri = kullaniciAdi.ToCharArray();
            foreach (char karakter in kAdKarakterleri)
            {
                int _asciiKod = (int)karakter;
                if (!((_asciiKod >= 65 && _asciiKod <= 90) || (_asciiKod >= 48 && _asciiKod <= 57) || (_asciiKod >= 97 && _asciiKod <= 122) || trKrkAscii.Contains(_asciiKod) ))
                    throw new Exception("Kullanıcı adı içerisinde özel karakter olamaz.");
            }
        }
        public static void PortveIpKontrol(string ip, int port)
        {
            if (ip == "" || ip.Contains(" "))
                throw new Exception("İp adresi boş bırakılamaz ve boşluk karakteri kullanılamaz.");

            
            
            IPAddress.Parse(ip);

            if (!(port > 1024 && port <= 65535))
                throw new Exception("PORT 1025 - 65535 arasında olmalıdır");
        }

        public static void kAdi_IP_PORTDoldur(Entry kAdi, Entry ip, Entry port)
        {

            kAdi.Text = "";
            string[] ipBolunmus = ipGetir().ToString().Split('.');
            ip.Text = ipBolunmus[0] + "." + ipBolunmus[1] + "." + ipBolunmus[2] + ".";
            port.Text = "6565";
        }

        public static IPAddress ipGetir()
        {
            IPHostEntry hostEntry = Dns.GetHostByName(Dns.GetHostName());
            IPAddress ipAdres = hostEntry.AddressList[hostEntry.AddressList.Length - 1];
            return ipAdres;
        }
    }
}
