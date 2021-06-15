using EmlakApi.Models;
using EmlakApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EmlakApi.Controllers
{
    public class ServisController : ApiController
    {
        EmlakAngularEntities db = new EmlakAngularEntities();
        SonucModel sonuc = new SonucModel();

        #region Kullanicilar
        [HttpGet]
        [Route("api/kullaniciliste")]
        public List<KullanicilarViewModel> KullaniciListe()
        {
            List<KullanicilarViewModel> kullanicilar = db.Kullanicilar.Select(x => new KullanicilarViewModel()
            {
                KullaniciId = x.KullaniciId,
                KullaniciAdi = x.KullaniciAdi,
                KullaniciMail = x.KullaniciMail,
                KullaniciSoyadi = x.KullaniciSoyadi,
                KullaniciSifre = x.KullaniciSifre,
                KullaniciTel = x.KullaniciTel,
                KullaniciTipId = x.KullaniciTipId,
                KullaniciTipAd = x.KullaniciTipler.KullaniciTipAd,
                KullaniciilanSay = x.Ilanlar.Count()
            }).OrderBy(s=>s.KullaniciTipAd).ToList();

            return kullanicilar;
        }

        [HttpGet]
        [Route("api/kullanicibyid/{kullaniciId}")]
        public KullanicilarViewModel Kullanicibyid(string kullaniciId)
        {
            KullanicilarViewModel kullanici = db.Kullanicilar.Where(s=>s.KullaniciId==kullaniciId).Select(x => new KullanicilarViewModel()
            {
                KullaniciId = x.KullaniciId,
                KullaniciAdi = x.KullaniciAdi,
                KullaniciMail = x.KullaniciMail,
                KullaniciSoyadi = x.KullaniciSoyadi,
                KullaniciSifre = x.KullaniciSifre,
                KullaniciTel = x.KullaniciTel,
                KullaniciTipId = x.KullaniciTipId,
                KullaniciTipAd = x.KullaniciTipler.KullaniciTipAd
            }).SingleOrDefault();

            return kullanici;
        }

        [HttpPost]
        [Route("api/kullaniciekle")]
        public SonucModel KullaniciEkle(Kullanicilar k)
        {
            Kullanicilar yenikullanici = new Kullanicilar()
            {
                KullaniciId = Guid.NewGuid().ToString(),
                KullaniciAdi=k.KullaniciAdi,
                KullaniciMail=k.KullaniciMail,
                KullaniciSoyadi=k.KullaniciSoyadi,
                KullaniciTipId=k.KullaniciTipId,
                KullaniciSifre=k.KullaniciSifre,
                KullaniciTel=k.KullaniciTel
            };

            if (db.Kullanicilar.Count(s=>s.KullaniciMail==k.KullaniciMail)>0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen E-Posta adresi başka kullanıcıda kayıtlıdır";
                return sonuc;
            }

            if (db.Kullanicilar.Count(s => s.KullaniciTel == k.KullaniciTel) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen TELEFON numarası başka kullanıcıda kayıtlıdır";
                return sonuc;
            }

            db.Kullanicilar.Add(yenikullanici);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kullanıcı başarıyla kaydedildi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/kullaniciduzenle")]
        public SonucModel KullaniciDuzenle(Kullanicilar k)
        {
            Kullanicilar duzenlekullanici = db.Kullanicilar.Where(s => s.KullaniciId == k.KullaniciId).SingleOrDefault();
            if (duzenlekullanici==null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kullanıcı bulunmadı";
                return sonuc;
            }
            if (db.Kullanicilar.Count(s => s.KullaniciMail == k.KullaniciMail && s.KullaniciId!=k.KullaniciId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen E-Posta adresi başka kullanıcıda kayıtlıdır";
                return sonuc;
            }

            if (db.Kullanicilar.Count(s => s.KullaniciTel == k.KullaniciTel && s.KullaniciId != k.KullaniciId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen TELEFON numarası başka kullanıcıda kayıtlıdır";
                return sonuc;
            }

            duzenlekullanici.KullaniciAdi = k.KullaniciAdi;
            duzenlekullanici.KullaniciMail = k.KullaniciMail;
            duzenlekullanici.KullaniciSoyadi = k.KullaniciSoyadi;
            duzenlekullanici.KullaniciTel = k.KullaniciTel;
            duzenlekullanici.KullaniciTipId = k.KullaniciTipId;
            duzenlekullanici.KullaniciSifre = k.KullaniciSifre;

            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kullanıcı düzenlendi.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/kullanicisil/{kullaniciId}")]
        public SonucModel KullaniciSil(string kullaniciId)
        {
            Kullanicilar silkullanici = db.Kullanicilar.Where(s => s.KullaniciId == kullaniciId).SingleOrDefault();
            if (silkullanici==null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kullanıcı bulunamadı";
                return sonuc;
            }
            if (db.Ilanlar.Count(s=>s.IlanSahibiId==kullaniciId)>0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üyeye ait ilan bulunmaktadır. Üye silinemez.";
                return sonuc;
            }

            db.Kullanicilar.Remove(silkullanici);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye silindi.";
            return sonuc;
        }
        #endregion

        #region Giris Kontrol

        [HttpGet]
        [Route("api/giriskontrol/{UyeMail}/{UyeSifre}")]
        public GirisKontrol GirisKontrol(string UyeMail,string UyeSifre)
        {
            GirisKontrol kullaniciGiris = db.Kullanicilar.Where(s => s.KullaniciMail == UyeMail && s.KullaniciSifre == UyeSifre).Select(x => new GirisKontrol()
            {
                KullaniciId=x.KullaniciId,
                KullaniciTipId=x.KullaniciTipId,
                KullaniciTipAd=x.KullaniciTipler.KullaniciTipAd
            }).FirstOrDefault();

            return kullaniciGiris;
        }
        #endregion

        #region İlanlar
        [HttpGet]
        [Route("api/ilanliste")]
        public List<IlanlarViewModel> IlanListe()
        {
            List<IlanlarViewModel> ilanlar = db.Ilanlar.Select(x => new IlanlarViewModel()
            {
                IlanId=x.IlanId,
                IlanAd=x.IlanAd,
                IlanILId=x.IlanILId,
                IlanILAd=x.Iller.ILAd,
                IlanILceId=x.IlanILceId,
                IlanILceAd=x.Ilceler.IlceAd,
                IlanAdres = x.IlanAdres,
                IlanEsyaId = x.IlanEsyaId,
                IlanEsyaAd=x.EsyaDurumu.EsyaAd,
                IlanOdaSayId=x.IlanOdaSayId,
                IlanOdaSayAd = x.OdaSayisi.OdaSayisiAd,
                IlanFiyat = x.IlanFiyat,
                IlanFotoUrl=x.IlanFotoUrl,
                IlanMKare=x.IlanMKare,
                IlanTipId=x.IlanTipId,
                IlanTipAd=x.IlanTip.TipAd,
                IlanYakitTipId=x.IlanYakitTipId,
                IlanYakitTipAd=x.YakitTipi.YakitTipAd,
                IlanSahibiId=x.IlanSahibiId
            }).ToList();

            foreach (var k in ilanlar)
            {
                k.kullaniciBilgi = Kullanicibyid(k.IlanSahibiId);
            }
            return ilanlar;
        }

        [HttpGet]
        [Route("api/ilanlistebyuid/{IlanSahibiId}")]
        public List<IlanlarViewModel> IlanByUidListe(string IlanSahibiId)
        {
            List<IlanlarViewModel> ilanlar = db.Ilanlar.Where(s=>s.IlanSahibiId==IlanSahibiId).Select(x => new IlanlarViewModel()
            {
                IlanId = x.IlanId,
                IlanAd = x.IlanAd,
                IlanILId = x.IlanILId,
                IlanILAd = x.Iller.ILAd,
                IlanILceId = x.IlanILceId,
                IlanILceAd = x.Ilceler.IlceAd,
                IlanAdres = x.IlanAdres,
                IlanEsyaId = x.IlanEsyaId,
                IlanEsyaAd = x.EsyaDurumu.EsyaAd,
                IlanOdaSayId = x.IlanOdaSayId,
                IlanOdaSayAd = x.OdaSayisi.OdaSayisiAd,
                IlanFiyat = x.IlanFiyat,
                IlanFotoUrl = x.IlanFotoUrl,
                IlanMKare = x.IlanMKare,
                IlanTipId = x.IlanTipId,
                IlanTipAd = x.IlanTip.TipAd,
                IlanYakitTipId = x.IlanYakitTipId,
                IlanYakitTipAd = x.YakitTipi.YakitTipAd,
                IlanSahibiId = x.IlanSahibiId
            }).ToList();

            foreach (var k in ilanlar)
            {
                k.kullaniciBilgi = Kullanicibyid(k.IlanSahibiId);
            }
            return ilanlar;
        }

        [HttpPost]
        [Route("api/ilanekle")]
        public SonucModel IlanEkle(Ilanlar ilan)
        {
            Ilanlar yeniilan = new Ilanlar()
            {
                IlanId = Guid.NewGuid().ToString(),
                IlanAd = ilan.IlanAd,
                IlanFiyat = ilan.IlanFiyat,
                IlanEsyaId = ilan.IlanEsyaId,
                IlanILId = ilan.IlanILId,
                IlanILceId = ilan.IlanILceId,
                IlanFotoUrl = ilan.IlanFotoUrl,
                IlanMKare = ilan.IlanMKare,
                IlanTipId = ilan.IlanTipId,
                IlanAdres = ilan.IlanAdres,
                IlanYakitTipId = ilan.IlanYakitTipId,
                IlanOdaSayId = ilan.IlanOdaSayId,
                IlanSahibiId = ilan.IlanSahibiId
            };
            if (db.Ilanlar.Count(s => s.IlanAd == ilan.IlanAd) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen ilan adında ilan bulunmaktadır.";
                return sonuc;
            }

            db.Ilanlar.Add(yeniilan);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "İlan başarıyla kaydedildi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/ilanduzenle")]
        public SonucModel IlanDuzenle(Ilanlar ilan)
        {
            Ilanlar duzenleilan = db.Ilanlar.Where(s => s.IlanId == ilan.IlanId).FirstOrDefault();

            if (duzenleilan==null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt bulunamadı.";
                return sonuc;
            }


            duzenleilan.IlanId = ilan.IlanId;
            duzenleilan.IlanAd = ilan.IlanAd;
            duzenleilan.IlanFiyat = ilan.IlanFiyat;
            duzenleilan.IlanEsyaId = ilan.IlanEsyaId;
            duzenleilan.IlanILId = ilan.IlanILId;
            duzenleilan.IlanILceId = ilan.IlanILceId;
            duzenleilan.IlanFotoUrl = ilan.IlanFotoUrl;
            duzenleilan.IlanMKare = ilan.IlanMKare;
            duzenleilan.IlanTipId = ilan.IlanTipId;
            duzenleilan.IlanAdres = ilan.IlanAdres;
            duzenleilan.IlanYakitTipId = ilan.IlanYakitTipId;
            duzenleilan.IlanOdaSayId = ilan.IlanOdaSayId;
            duzenleilan.IlanSahibiId = ilan.IlanSahibiId;
           
            if (db.Ilanlar.Count(s => s.IlanAd == ilan.IlanAd && s.IlanId!=ilan.IlanId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen ilan adında ilan bulunmaktadır.";
                return sonuc;
            }

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "İlan başarıyla kaydedildi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/ilansil/{IlanId}")]
        public SonucModel IlanSil(string IlanId)
        {
            Ilanlar sililan = db.Ilanlar.Where(s => s.IlanId == IlanId).FirstOrDefault();

            if (sililan==null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "İlan bulunamadı.";
                return sonuc;
            }

            db.Ilanlar.Remove(sililan);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "İlan başarıyla silindi.";
            return sonuc;

        }

        #endregion

        #region Ilantipleri
        [HttpGet]
        [Route("api/ilantipliste")]
        public List<IlanTipViewModel> IlanTipListe()
        {
            List<IlanTipViewModel> ilantipler = db.IlanTip.Select(x => new IlanTipViewModel()
            {
                TipId = x.TipId,
                TipAd = x.TipAd
            }).OrderBy(s=>s.TipAd).ToList();

            return ilantipler;
        }

        [HttpGet]
        [Route("api/ilantipbyid/{TipId}")]
        public IlanTipViewModel IlanTipbyid(string TipId)
        {
            IlanTipViewModel ilantip = db.IlanTip.Where(s => s.TipId == TipId).Select(x => new IlanTipViewModel()
            {
                TipId = x.TipId,
                TipAd = x.TipAd
            }).SingleOrDefault();

            return ilantip;
        }

        [HttpPost]
        [Route("api/ilantipekle")]
        public SonucModel IlanTipEkle(IlanTip tip)
        {
            IlanTip yeniilantip = new IlanTip()
            {
                TipId = Guid.NewGuid().ToString(),
                TipAd = tip.TipAd
            };

            if (db.IlanTip.Count(s => s.TipAd == tip.TipAd) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen tip adı kayıtlıdır";
                return sonuc;
            }

            db.IlanTip.Add(yeniilantip);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "İlan tipi başarıyla kaydedildi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/ilantipduzenle")]
        public SonucModel IlanTipDuzenle(IlanTip tip)
        {
            IlanTip duzenletip = db.IlanTip.Where(s => s.TipId == tip.TipId).SingleOrDefault();
            if (duzenletip == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kullanıcı bulunmadı";
                return sonuc;
            }
            if (db.IlanTip.Count(s => s.TipAd == tip.TipAd && s.TipId != tip.TipId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "İlan tipi mevcuttur.";
                return sonuc;
            }

            duzenletip.TipAd = tip.TipAd;

            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "İlan tipi başarıyla düzenlendi.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/ilantipisil/{TipId}")]
        public SonucModel IlantipSil(string TipId)
        {
            IlanTip siltip = db.IlanTip.Where(s => s.TipId == TipId).SingleOrDefault();
            if (siltip == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "İlan tipi bulunamadı";
                return sonuc;
            }
            if (db.Ilanlar.Count(s => s.IlanTipId == TipId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu ilan tipine kayıtlı ilan bulunmaktadır. Silinemez.";
                return sonuc;
            }

            db.IlanTip.Remove(siltip);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "İlan tipi silindi.";
            return sonuc;
        }
        #endregion

        #region Eşyadurumu
        [HttpGet]
        [Route("api/esyadurumuliste")]
        public List<EsyaDurumuViewModel> EsyaDurumuListe()
        {
            List<EsyaDurumuViewModel> esyadurumliste = db.EsyaDurumu.Select(x => new EsyaDurumuViewModel()
            {
                EsyaId=x.EsyaId,
                EsyaAd=x.EsyaAd
            }).ToList();

            return esyadurumliste;
        }

        [HttpGet]
        [Route("api/esyadurumbyid/{EsyaId}")]
        public EsyaDurumuViewModel EsyaDurumbyid(string EsyaId)
        {
            EsyaDurumuViewModel esyadurum = db.EsyaDurumu.Where(s => s.EsyaId == EsyaId).Select(x => new EsyaDurumuViewModel()
            {
                EsyaId = x.EsyaId,
                EsyaAd = x.EsyaAd
            }).SingleOrDefault();

            return esyadurum;
        }

        [HttpPost]
        [Route("api/esyadurumuekle")]
        public SonucModel EsyaDurumuEkle(EsyaDurumu esya)
        {
            EsyaDurumu yeniesyadurum = new EsyaDurumu()
            {
                EsyaId = Guid.NewGuid().ToString(),
                EsyaAd=esya.EsyaAd
            };

            if (db.EsyaDurumu.Count(s => s.EsyaAd == esya.EsyaAd) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen eşya durumu kayıtlıdır";
                return sonuc;
            }

            db.EsyaDurumu.Add(yeniesyadurum);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Eşya durumu başarıyla eklendi.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/esyadurumduzenle")]
        public SonucModel EsyaDurumDuzenle(EsyaDurumu esya)
        {
            EsyaDurumu duzenleesya = db.EsyaDurumu.Where(s => s.EsyaId == esya.EsyaId).SingleOrDefault();
            if (duzenleesya == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "kayıt bulunmadı";
                return sonuc;
            }
            if (db.EsyaDurumu.Count(s => s.EsyaAd == esya.EsyaAd && s.EsyaId != esya.EsyaId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Eşya bilgisi mevcuttur.";
                return sonuc;
            }

            duzenleesya.EsyaAd = esya.EsyaAd;

            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Eşya durumu başarıyla düzenlendi.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/esyadurumusil/{EsyaId}")]
        public SonucModel esyaSil(string EsyaId)
        {
            EsyaDurumu silesya = db.EsyaDurumu.Where(s => s.EsyaId == EsyaId).SingleOrDefault();
            if (silesya == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt bulunamadı";
                return sonuc;
            }
            if (db.Ilanlar.Count(s => s.IlanEsyaId == EsyaId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu eşya durumunda kayıtlı ilan bulunmaktadır. Silinemez.";
                return sonuc;
            }

            db.EsyaDurumu.Remove(silesya);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Eşya durumu silindi.";
            return sonuc;
        }
        #endregion

        #region OdaSayısı
        [HttpGet]
        [Route("api/odasayisiliste")]
        public List<OdaSayisiViewModel> OdaSayisiListe()
        {
            List<OdaSayisiViewModel> odasayisiliste = db.OdaSayisi.Select(x => new OdaSayisiViewModel()
            {
                OdaSayisiId=x.OdaSayisiId,
                OdaSayisiAd=x.OdaSayisiAd
            }).ToList();

            return odasayisiliste;
        }

        [HttpGet]
        [Route("api/odasayisibyid/{OdaSayisiId}")]
        public OdaSayisiViewModel OdaSayisibyid(string OdaSayisiId)
        {
            OdaSayisiViewModel odasayisi = db.OdaSayisi.Where(s => s.OdaSayisiId == OdaSayisiId).Select(x => new OdaSayisiViewModel()
            {
                OdaSayisiId=x.OdaSayisiId,
                OdaSayisiAd=x.OdaSayisiAd
            }).SingleOrDefault();

            return odasayisi;
        }

        [HttpPost]
        [Route("api/odasayisiekle")]
        public SonucModel OdaSayisiEkle(OdaSayisi oda)
        {
            OdaSayisi yeniodasayisi = new OdaSayisi()
            {
                OdaSayisiId = Guid.NewGuid().ToString(),
                OdaSayisiAd = oda.OdaSayisiAd
            };

            if (db.OdaSayisi.Count(s => s.OdaSayisiAd == oda.OdaSayisiAd) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen oda tipi kayıtlıdır";
                return sonuc;
            }

            db.OdaSayisi.Add(yeniodasayisi);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Oda sayısı bilgisi başarıyla eklendi.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/odasayisiduzenle")]
        public SonucModel OdaSayisiDuzenle(OdaSayisi oda)
        {
            OdaSayisi duzenleoda = db.OdaSayisi.Where(s => s.OdaSayisiId == oda.OdaSayisiId).SingleOrDefault();
            if (duzenleoda == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "kayıt bulunmadı";
                return sonuc;
            }
            if (db.OdaSayisi.Count(s => s.OdaSayisiAd == oda.OdaSayisiAd && s.OdaSayisiId != oda.OdaSayisiId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Oda sayısı bilgisi mevcuttur.";
                return sonuc;
            }

            duzenleoda.OdaSayisiAd = oda.OdaSayisiAd;

            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Oda Sayısı bilgisi başarıyla düzenlendi.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/odasayisisil/{OdaSayisiId}")]
        public SonucModel OdaSayisiSil(string OdaSayisiId)
        {
            OdaSayisi siloda = db.OdaSayisi.Where(s => s.OdaSayisiId == OdaSayisiId).SingleOrDefault();
            if (siloda == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt bulunamadı";
                return sonuc;
            }
            if (db.Ilanlar.Count(s => s.IlanOdaSayId == OdaSayisiId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu oda sayısına kayıtlı ilan bulunmaktadır. Silinemez.";
                return sonuc;
            }

            db.OdaSayisi.Remove(siloda);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Oda bilgisi silindi.";
            return sonuc;
        }
        #endregion

        #region YakıtTipi
        [HttpGet]
        [Route("api/yakittipiliste")]
        public List<YakitTipiViewModel> YakitTipiListe()
        {
            List<YakitTipiViewModel> yakittipiliste = db.YakitTipi.Select(x => new YakitTipiViewModel()
            {
                YakitTipId=x.YakitTipId,
                YakitTipAd=x.YakitTipAd
            }).ToList();

            return yakittipiliste;
        }

        [HttpGet]
        [Route("api/yakittipibyid/{YakitTipId}")]
        public YakitTipiViewModel YakitTipibyid(string YakitTipId)
        {
            YakitTipiViewModel yakittipi = db.YakitTipi.Where(s => s.YakitTipId == YakitTipId).Select(x => new YakitTipiViewModel()
            {
                YakitTipId = x.YakitTipId,
                YakitTipAd=x.YakitTipAd
            }).SingleOrDefault();

            return yakittipi;
        }

        [HttpPost]
        [Route("api/yakittipiekle")]
        public SonucModel YakitTipiEkle(YakitTipi yakit)
        {
            YakitTipi yeniyakittipi = new YakitTipi()
            {
                YakitTipId = Guid.NewGuid().ToString(),
                YakitTipAd = yakit.YakitTipAd
            };

            if (db.YakitTipi.Count(s => s.YakitTipAd == yakit.YakitTipAd) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen yakit tipi kayıtlıdır";
                return sonuc;
            }

            db.YakitTipi.Add(yeniyakittipi);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Yakıt Tipi bilgisi başarıyla eklendi.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/yakittipiduzenle")]
        public SonucModel YakitTipiDuzenle(YakitTipi yakit)
        {
            YakitTipi duzenleyakit = db.YakitTipi.Where(s => s.YakitTipId == yakit.YakitTipId).SingleOrDefault();
            if (duzenleyakit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "kayıt bulunmadı";
                return sonuc;
            }
            if (db.YakitTipi.Count(s => s.YakitTipAd == yakit.YakitTipAd && s.YakitTipId != yakit.YakitTipId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Yakıt Tipi bilgisi mevcuttur.";
                return sonuc;
            }

            duzenleyakit.YakitTipAd = yakit.YakitTipAd;

            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Yakıt Tipi bilgisi başarıyla düzenlendi.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/yakittipisil/{YakitTipId}")]
        public SonucModel YakitTipiSil(string YakitTipId)
        {
            YakitTipi silyakit = db.YakitTipi.Where(s => s.YakitTipId == YakitTipId).SingleOrDefault();
            if (silyakit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt bulunamadı";
                return sonuc;
            }
            if (db.Ilanlar.Count(s => s.IlanYakitTipId == YakitTipId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu yakıt tipinde kayıtlı ilan bulunmaktadır. Silinemez.";
                return sonuc;
            }

            db.YakitTipi.Remove(silyakit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Yakıt Tipi bilgisi silindi.";
            return sonuc;
        }
        #endregion

        #region KullaniciTipi
        [HttpGet]
        [Route("api/kullanicitipiliste")]
        public List<KullaniciTiplerViewModel> KullaniciTipiListe()
        {
            List<KullaniciTiplerViewModel> kullanicitipiliste = db.KullaniciTipler.Select(x => new KullaniciTiplerViewModel()
            {
               KullaniciTipId=x.KullaniciTipId,
               KullaniciTipAd=x.KullaniciTipAd
            }).ToList();

            return kullanicitipiliste;
        }

        [HttpGet]
        [Route("api/kullanicitipibyid/{KullaniciTipId}")]
        public KullaniciTiplerViewModel KullaniciTipibyid(string KullaniciTipId)
        {
            KullaniciTiplerViewModel kullanicitipi = db.KullaniciTipler.Where(s => s.KullaniciTipId == KullaniciTipId).Select(x => new KullaniciTiplerViewModel()
            {
                KullaniciTipId = x.KullaniciTipId,
                KullaniciTipAd = x.KullaniciTipAd
            }).SingleOrDefault();

            return kullanicitipi;
        }

        [HttpPost]
        [Route("api/kullanicitipiekle")]
        public SonucModel KullaniciTipiEkle(KullaniciTipler kt)
        {
            KullaniciTipler yenikullanicitipi = new KullaniciTipler()
            {
                KullaniciTipId = Guid.NewGuid().ToString(),
                KullaniciTipAd = kt.KullaniciTipAd
            };

            if (db.KullaniciTipler.Count(s => s.KullaniciTipAd == kt.KullaniciTipAd) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen kullanıcı tipi kayıtlıdır";
                return sonuc;
            }

            db.KullaniciTipler.Add(yenikullanicitipi);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kullanıcı Tipi bilgisi başarıyla eklendi.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/kullanicitipiduzenle")]
        public SonucModel KullaniciTipiDuzenle(KullaniciTipler kt)
        {
            KullaniciTipler duzenlekt = db.KullaniciTipler.Where(s => s.KullaniciTipId == kt.KullaniciTipId).SingleOrDefault();
            if (duzenlekt == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "kayıt bulunamadı";
                return sonuc;
            }
            if (db.KullaniciTipler.Count(s => s.KullaniciTipAd == kt.KullaniciTipAd && s.KullaniciTipId != kt.KullaniciTipId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kullanıcı Tipi bilgisi mevcuttur.";
                return sonuc;
            }

            duzenlekt.KullaniciTipAd = kt.KullaniciTipAd;

            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kullanıcı Tipi bilgisi başarıyla düzenlendi.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/kullanicitipisil/{KullaniciTipId}")]
        public SonucModel KullaniciTipiSil(string KullaniciTipId)
        {
            KullaniciTipler silkt = db.KullaniciTipler.Where(s => s.KullaniciTipId == KullaniciTipId).SingleOrDefault();
            if (silkt == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt bulunamadı";
                return sonuc;
            }
            if (db.Kullanicilar.Count(s => s.KullaniciTipId == KullaniciTipId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu kullanıcı tipinde kayıtlı kullanıcı bulunmaktadır. Silinemez.";
                return sonuc;
            }

            db.KullaniciTipler.Remove(silkt);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kullanıcı Tipi bilgisi silindi.";
            return sonuc;
        }
        #endregion

        #region İller
        [HttpGet]
        [Route("api/illerliste")]
        public List<IllerViewModel> IllerListe()
        {
            List<IllerViewModel> illerliste = db.Iller.Select(x => new IllerViewModel()
            {
                ILId=x.ILId,
                ILAd=x.ILAd
            }).ToList();

            return illerliste;
        }

        [HttpGet]
        [Route("api/Illerbyid/{ILId}")]
        public IllerViewModel Illerbyid(string ILId)
        {
            IllerViewModel il = db.Iller.Where(s => s.ILId == ILId).Select(x => new IllerViewModel()
            {
                ILId = x.ILId,
                ILAd = x.ILAd
            }).SingleOrDefault();

            return il;
        }

        [HttpPost]
        [Route("api/ilekle")]
        public SonucModel ilEkle(Iller il)
        {
            Iller yeniil = new Iller()
            {
                ILId = Guid.NewGuid().ToString(),
                ILAd = il.ILAd
            };

            if (db.Iller.Count(s => s.ILAd == il.ILAd) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen il adı kayıtlıdır";
                return sonuc;
            }

            db.Iller.Add(yeniil);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "İl bilgisi başarıyla eklendi.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/ilduzenle")]
        public SonucModel ilDuzenle(Iller il)
        {
            Iller duzenleil = db.Iller.Where(s => s.ILId == il.ILId).SingleOrDefault();
            if (duzenleil == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "kayıt bulunamadı";
                return sonuc;
            }
            if (db.Iller.Count(s => s.ILAd == il.ILAd && s.ILId != il.ILId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "İl adı mevcuttur.";
                return sonuc;
            }

            duzenleil.ILAd = il.ILAd;

            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "İl adı başarıyla düzenlendi.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/ilsil/{ILId}")]
        public SonucModel ilSil(string ILId)
        {
            Iller silil = db.Iller.Where(s => s.ILId == ILId).SingleOrDefault();
            if (silil == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt bulunamadı";
                return sonuc;
            }
            if (db.Ilanlar.Count(s => s.IlanILId == ILId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu il adına kayıtlı ilan bulunmaktadır. Silinemez.";
                return sonuc;
            }
            if (db.Ilceler.Count(s => s.IlceIlid == ILId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu ilde kayıtlı ilçeler bulunmaktadır. Silinemez.";
                return sonuc;
            }

            db.Iller.Remove(silil);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "İl bilgisi silindi.";
            return sonuc;
        }
        #endregion

        #region İlçeler
        [HttpGet]
        [Route("api/ilcelerliste")]
        public List<IlcelerViewModel> IlcelerListe()
        {
            List<IlcelerViewModel> ilcelerliste = db.Ilceler.Select(x => new IlcelerViewModel()
            {
              IlceId=x.IlceId,
              IlceAd=x.IlceAd,
              IlceIlid=x.IlceIlid,
              IlceIlAd=x.Iller.ILAd
            }).ToList();

            return ilcelerliste;
        }

        [HttpGet]
        [Route("api/Ilcelerbyid/{IlceId}")]
        public IlcelerViewModel Ilcelerbyid(string IlceId)
        {
            IlcelerViewModel ilce = db.Ilceler.Where(s => s.IlceId == IlceId).Select(x => new IlcelerViewModel()
            {
                IlceId = x.IlceId,
                IlceAd = x.IlceAd,
                IlceIlid = x.IlceIlid,
                IlceIlAd = x.Iller.ILAd
            }).SingleOrDefault();

            return ilce;
        }

        [HttpGet]
        [Route("api/Ilcelerbyilid/{IlId}")]
        public List<IlcelerViewModel> Ilcelerbyilid(string IlId)
        {
            List<IlcelerViewModel> ilce = db.Ilceler.Where(s => s.IlceIlid == IlId).Select(x => new IlcelerViewModel()
            {
                IlceId = x.IlceId,
                IlceAd = x.IlceAd,
                IlceIlid = x.IlceIlid,
                IlceIlAd = x.Iller.ILAd
            }).ToList();

            return ilce;
        }
        [HttpPost]
        [Route("api/ilceekle")]
        public SonucModel ilceEkle(Ilceler ilce)
        {
            Ilceler yeniilce = new Ilceler()
            {
                IlceId = Guid.NewGuid().ToString(),
                IlceAd =ilce.IlceAd,
                IlceIlid=ilce.IlceIlid
            };

            if (db.Ilceler.Count(s => s.IlceAd == ilce.IlceAd && s.IlceIlid==ilce.IlceIlid) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen ilce adı kayıtlıdır";
                return sonuc;
            }

            db.Ilceler.Add(yeniilce);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "İlce bilgisi başarıyla eklendi.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/ilceduzenle")]
        public SonucModel ilceDuzenle(Ilceler ilce)
        {
            Ilceler duzenleilce = db.Ilceler.Where(s => s.IlceId == ilce.IlceId).SingleOrDefault();
            if (duzenleilce == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "kayıt bulunamadı";
                return sonuc;
            }
            if (db.Ilceler.Count(s => s.IlceAd == ilce.IlceAd && s.IlceIlid==ilce.IlceIlid && s.IlceId != ilce.IlceId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "İlçe adı mevcuttur.";
                return sonuc;
            }

            duzenleilce.IlceAd = ilce.IlceAd;
            duzenleilce.IlceIlid = ilce.IlceIlid;

            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "İlce adı başarıyla düzenlendi.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/ilcesil/{IlceId}")]
        public SonucModel ilceSil(string IlceId)
        {
            Ilceler sililce = db.Ilceler.Where(s => s.IlceId == IlceId).SingleOrDefault();
            if (sililce == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt bulunamadı";
                return sonuc;
            }
            if (db.Ilanlar.Count(s => s.IlanILceId == IlceId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu ilce adına kayıtlı ilan bulunmaktadır. Silinemez.";
                return sonuc;
            }

            db.Ilceler.Remove(sililce);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "İlçe bilgisi silindi.";
            return sonuc;
        }
        #endregion

        //de

    }
}
