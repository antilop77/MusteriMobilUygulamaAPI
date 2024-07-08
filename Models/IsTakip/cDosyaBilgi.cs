namespace MusteriMobilUygulamaAPI.Models.IsTakip
{
    public class cDosyaBilgi
    {
        public Beyanname? beyanname { get; set; }
        public List<MusteriTemsilcisi>? musteri_temsilcisi { get; set; }
        public List<Data>? data { get; set; }
    }
    public class Beyanname
    {
        public string? beyanname_no { get; set; }
    }
    public class MusteriTemsilcisi
    {
        public string? personel { get; set; }
        public string? kodu { get; set; }
        public string? mail { get; set; }
        public string? cep_no1 { get; set; }
        public string? dahili_no { get; set; }
        public string? kisa_kod { get; set; }
    }
    public class Data
    {
        public string? tip { get; set; }
        public string? aciklama { get; set; }
        public string? bilgi { get; set; }
    }
}