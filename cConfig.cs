namespace MmuAPI
{
    public class cConfig
    {
        public static string sUrlForApi_MailAuth = "";
        public static string sConnectionString = "";
        public static string sConnectionStringForLoggerX = "";
        public static string sConnectionStringForKurumsal = "";
        public static string sConnectionStringForIdari = "";
        public static string sDosyaYoluDilekce = "";
        public static string sDosyaYoluTarim = "";
        public static string sShared_Dir = "";
        public static string sEnvProdOrTest = "";

        public static string sArsivPath = "";
        public static string sMobilPath = "";
        public static string sWinLoginUser = "";
        public static string sWinLoginPass = "";
        public string sAppPath = "";

        public string sGrant_type = "";
        public string sClient_id = "";
        public string sClient_secret = "";
        public string sScope = "";
        public string? sToken = "";
        public string sUrlGetUsers = "";
        public string sUrlGetUser = "";
    }

    public class cToken
    {
        public string? access_token { get; set; }
        public int expires_in { get; set; }
        public string? token_type { get; set; }
        public string? scope { get; set; }
    }
}
