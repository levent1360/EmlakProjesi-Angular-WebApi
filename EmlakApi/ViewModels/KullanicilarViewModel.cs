using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmlakApi.ViewModels
{
    public class KullanicilarViewModel
    {
        public string KullaniciId { get; set; }
        public string KullaniciAdi { get; set; }
        public string KullaniciSoyadi { get; set; }
        public string KullaniciMail { get; set; }
        public string KullaniciSifre { get; set; }
        public string KullaniciTipId { get; set; }
        public string KullaniciTipAd { get; set; }
        public string KullaniciTel { get; set; }
        public int KullaniciilanSay { get; set; }

    }
}