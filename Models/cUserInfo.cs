using MusteriMobilUygulamaAPI.Models.IsTakip;

namespace MmuAPI.Models
{
    public class cUserInfo
    {
        public int Users_Id { get; set; }
        public string? kodu { get; set; }
        public string? AdiSoyadi { get; set; }
        public string? email { get; set; }
        public string? FormName { get; set; }
        public string? TCKimlik { get; set; }
        public int? DusunulenKuryeNo { get; set; }
        public string? DepartmanAdi { get; set; }
        public string? GorevTanimi { get; set; }
        public string? Sektor { get; set; }
        public string? bolge { get; set; }
        public string? sektorkod { get; set; }
        public string? departmankod { get; set; }
        public string? bolgekod { get; set; }
        public string? NotificationChannels { get; set; }
        public List<cUserAuthorizedForms>? UserAuthorizedForms { get; set;}
        public List<cFirmalar>? firmalar { get; set;}
        public List<cKullaniciBilgileri>? message_users { get; set;}
        public string? user_image { get; set; }        
    }

    public class cUserAuthorizedForms
    {
        public string? formId { get; set;}
        public bool? formAccess { get; set;}
    }
}
