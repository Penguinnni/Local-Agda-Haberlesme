using System;
using System.Collections.Generic;
using System.Text;

namespace LHMobil.Model
{
    public class CevrimiciK
    {
        public string KullaniciAdi { get; set; }
        public int BildirimSayi { get; set; }

        public string Bildirim
        {
            get
            {
                if (BildirimSayi > 0)
                    return "🔔" + BildirimSayi;
                else
                    return null;
            }
        }
    }
}
