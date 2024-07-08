namespace MusteriMobilUygulamaAPI.Models.IsTakip
{
    public class cTarimOdemeKayitGorseliYukle
    {
        public string? dosya_no { get; set; }
        public string? kullanici_kodu { get; set; }
        public string? urun_bildirim_no { get; set; }
        public string? kayit_no { get; set; }
        public List<string?>? images { get; set; }
        public string? odeme_tipi { get; set; }
        public string? odeme_tipi_id { get; set; }
    }
}