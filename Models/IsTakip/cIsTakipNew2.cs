using System;

namespace MusteriMobilUygulamaAPI.Models.IsTakip
{
    public class cIsTakipNew2
    {
        public string? kod { get; set; }
        public string? imei { get; set; }
        public int okuma_tipi { get; set; }
        public string? is_takip_kodu { get; set; }
        public string? aciklama { get; set; }
        public string? dosya_no { get; set; }
        public string? resim { get; set; }
        public string? resim_aciklama { get; set; }
        public string? gecikme_sebebi { get; set; }
        public string? adet { get; set; }
        public string? gercek_tarih_saat { get; set; }
    }
}