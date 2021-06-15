using EmlakApi.Models;
using EmlakApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmlakApi.Auth
{
    public class kullaniciService
    {
        EmlakAngularEntities db = new EmlakAngularEntities();

        public KullanicilarViewModel OturumAc(string KullaniciMail,string KullaniciSifre)
        {
            KullanicilarViewModel giriskullanici = db.Kullanicilar.Where(s => s.KullaniciMail == KullaniciMail && s.KullaniciSifre == KullaniciSifre).Select(x => new KullanicilarViewModel()
            {
                KullaniciId = x.KullaniciId,
                KullaniciAdi = x.KullaniciAdi,
                KullaniciMail = x.KullaniciMail,
                KullaniciSoyadi = x.KullaniciSoyadi,
                KullaniciSifre = x.KullaniciSifre,
                KullaniciTel = x.KullaniciTel,
                KullaniciTipId = x.KullaniciTipId,
                KullaniciTipAd = x.KullaniciTipler.KullaniciTipAd
            }).FirstOrDefault();
            return giriskullanici;
        }
    }
}