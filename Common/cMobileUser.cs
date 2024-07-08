using System;

namespace MusteriMobilUygulamaAPI.Common
{
    public class cMobileUser
    {
        public int Id { get; set; }
        public string? Kodu { get; set; }
        public string? AdSoyad { get; set; }
        public string? Email { get; set; }
        public string? UDID { get; set; }
        public int FirmaNo { get; set; }
        public string? FirmaUnvan { get; set; }
        public string? Adres { get; set; }
        public string? Tel { get; set; }
        public string? Fax { get; set; }
    }
}
