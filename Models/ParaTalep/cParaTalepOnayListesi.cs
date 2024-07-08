namespace MusteriMobilUygulamaAPI.Models.ParaTalep
{
    public class cParaTalepOnayListesi
    {
        public int ID { get; set; }
        public int? Detay_No { get; set; }
        public string? Tip { get; set; }
        public string? ReferansNo { get; set; }
        public string? Musteri { get; set; }
        public string? Tanim { get; set; }
        public double? Tutar { get; set; }
        public string? Doviz_Kodu { get; set; }
    }
}