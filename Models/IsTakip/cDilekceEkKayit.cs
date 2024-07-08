namespace MusteriMobilUygulamaAPI.Models.IsTakip
{
    public class cDilekceEkKayit
    {
        public int id { get; set; }
        public string? kayitNo { get; set; }
        public string? tarih { get; set; }
        public string? kullaniciKodu { get; set; }
        public required List<string?> resimListesi { get; set; }
    }
}