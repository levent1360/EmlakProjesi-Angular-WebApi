//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmlakApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ilanlar
    {
        public string IlanId { get; set; }
        public string IlanAd { get; set; }
        public int IlanFiyat { get; set; }
        public int IlanMKare { get; set; }
        public string IlanILId { get; set; }
        public string IlanILceId { get; set; }
        public string IlanTipId { get; set; }
        public string IlanEsyaId { get; set; }
        public string IlanYakitTipId { get; set; }
        public string IlanOdaSayId { get; set; }
        public string IlanFotoUrl { get; set; }
        public string IlanAdres { get; set; }
        public string IlanSahibiId { get; set; }
    
        public virtual EsyaDurumu EsyaDurumu { get; set; }
        public virtual IlanTip IlanTip { get; set; }
        public virtual Ilceler Ilceler { get; set; }
        public virtual Iller Iller { get; set; }
        public virtual Kullanicilar Kullanicilar { get; set; }
        public virtual OdaSayisi OdaSayisi { get; set; }
        public virtual YakitTipi YakitTipi { get; set; }
    }
}
