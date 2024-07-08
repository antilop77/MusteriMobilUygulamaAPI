using System;

namespace MusteriMobilUygulamaAPI.Common
{
    public class cUgmUser
    {
        public string? AdSoyad { get; set; }
        public string? TcKimlik { get; set; }
        public string? FirmaUnvan { get; set; }
        public required string Kodu { get; set; }
    }
}
