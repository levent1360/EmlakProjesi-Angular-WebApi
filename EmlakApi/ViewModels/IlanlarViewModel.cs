using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmlakApi.ViewModels
{
    public class IlanlarViewModel
    {
        public string IlanId { get; set; }
        public string IlanAd { get; set; }
        public int IlanFiyat { get; set; }
        public int IlanMKare { get; set; }
        public string IlanILId { get; set; }
        public string IlanILAd { get; set; }
        public string IlanILceId { get; set; }
        public string IlanILceAd { get; set; }
        public string IlanTipId { get; set; }
        public string IlanTipAd { get; set; }
        public string IlanEsyaId { get; set; }
        public string IlanEsyaAd { get; set; }
        public string IlanYakitTipId { get; set; }
        public string IlanYakitTipAd { get; set; }
        public string IlanOdaSayId { get; set; }
        public string IlanOdaSayAd { get; set; }
        public string IlanFotoUrl { get; set; }
        public string IlanAdres { get; set; }
        public string IlanSahibiId { get; set; }
        public KullanicilarViewModel kullaniciBilgi { get; set; }
    }
}