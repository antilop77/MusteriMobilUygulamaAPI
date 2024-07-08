namespace MmuAPI.Models
{
    public class cUserCredential
    {
        public int? in_user_id { get; set;}
        public int? ex_user_id { get; set;}
        public string? imei { get; set;}
        public string? email { get; set;}
        public bool externalx { get; set;}
        public bool internalx { get; set;}
    }
}