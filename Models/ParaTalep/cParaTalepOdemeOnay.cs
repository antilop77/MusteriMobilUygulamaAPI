namespace MusteriMobilUygulamaAPI.Models.ParaTalep
{
    public class cParaTalepOdemeOnay
    {
        public int Detay_No { get; set; }
        public string? IsiVeren { get; set; }
        public string? Tip { get; set; }
        public string? ReferansNo { get; set; }
        public string? Musteri { get; set; }
        public Nullable<System.DateTime> GirisTarih { get; set; }
        public string? GirisSaati { get; set; }
        public Nullable<double> Tutar { get; set; }
        public string? Doviz { get; set; }
        public string? Cins { get; set; }
        public string? CinsAciklama { get; set; }
        public string? ParayiAlanHesapNo { get; set; }
        public string? HesapAdi { get; set; }
        public string? Kayit_tarihi { get; set; }
        public string? MC { get; set; }
        public Nullable<double> Bakiye { get; set; }
        public Nullable<bool> UnspedOnay { get; set; }
        public string? UnspedOnayKulKod { get; set; }
        public Nullable<System.DateTime> UnspedOnayTarih { get; set; }
    }
}
