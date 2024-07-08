namespace MusteriMobilUygulamaAPI.Models.Intranet
{
    public class cHaftalikYemek
    {
        public int YemekID { get; set; }
        public string? Adi { get; set; }            
        public string? Aciklama { get; set; }  
        public DateTime? ServisTarihi { get; set; }  
        public int YemekTipId { get; set; }  
        public string? TipAdi { get; set; }  
    }	
}
