namespace MusteriMobilUygulamaAPI.Models.Api
{
    public class cSendMail
    {
        public string? from { get;set;}
        public string? password { get;set;}
        public string? cc { get;set;}
        public string? to { get;set;}
        public string? bcc { get;set;}
        public string? subject { get;set;}
        public string? body { get;set;}
        public string? displayName { get;set;}
    }
}
