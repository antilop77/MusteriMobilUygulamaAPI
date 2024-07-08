using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using MmuAPI;
using MmuAPI.Models;
using MusteriMobilUygulamaAPI.Models;

namespace MusteriMobilUygulamaAPI.Services
{
    public static class sLogin
    {
        public static string checkUserPassword(cUser pUser, HttpContext pHttpContext)
        {
            //int y=0, x = 1/y;
            try
            {
                cLogin item = new cLogin();

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = cConfig.sConnectionString; ;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    string sql = @" select isnull(internal, -1) internal, isnull(internal_iptal, '') internal_iptal, isnull([external], -1) [external], isnull(external_iptal, 0) external_iptal,	email from (select
                                    (select top 1 u.Users_ID from evrim_db.dbo.users u where u.Kodu = '" + pUser.username + "' and u.Sifre = '" + pUser.password + "' and u.IMEI ='" + pUser.guid + @"') as internal,
                                    (select top 1 ISNULL(u.Mobil,'') Iptal from evrim_db.dbo.users u where u.Kodu = '" + pUser.username + "' and u.Sifre = '" + pUser.password + "' and u.IMEI ='" + pUser.guid + @"' and ISNULL(Mobil,1)=0) as internal_iptal,  
                                    (select top 1 a.ID  from evrim_db.dbo.MobilKullanici a where a.Kodu = '" + pUser.username + "' and a.Parola = '" + pUser.password + "' and a.UDID ='" + pUser.guid + @"') as [external],
                                    (select top 1 ISNULL(a.Pasif,'') Iptal  from evrim_db.dbo.MobilKullanici a where a.Kodu = '" + pUser.username + "' and a.Parola = '" + pUser.password + "' and a.UDID ='" + pUser.guid + @"' and ISNULL(Pasif,0)=1) as [external_iptal],
                                    isnull(case 
                                    when isnull((select mail from evrim_db.dbo.users u where u.Kodu = '" + pUser.username + "' and u.Sifre = '" + pUser.password + "' and u.IMEI ='" + pUser.guid + @"'),'') != '' 
                                    then (select mail from evrim_db.dbo.users u where u.Kodu = '" + pUser.username + "' and u.Sifre = '" + pUser.password + "' and u.IMEI = '" + pUser.guid + @"')
                                    else (select top 1 email  from evrim_db.dbo.MobilKullanici a where a.Kodu = '" + pUser.username + "' and a.Parola = '" + pUser.password + "' and a.UDID = '" + pUser.guid + "')end,'') email) aa ";

                    cUserCredential oUserCredential = new cUserCredential();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                item.internall = reader.GetInt32(0);
                                oUserCredential.in_user_id = item.internall;
                                item.internal_iptal = reader.GetString(1);
                                item.externall = reader.GetInt32(2);
                                oUserCredential.ex_user_id = item.externall;
                                item.external_iptal = reader.GetBoolean(3);
                                item.email = reader.GetString(4);
                                oUserCredential.email = item.email;
                                oUserCredential.imei = pUser.guid;
                                oUserCredential.internalx = false;
                                if (item.internall != -1)
                                    oUserCredential.internalx = true;
                                oUserCredential.externalx = false;
                                if (item.externall != -1)
                                    oUserCredential.externalx = true;

                                pHttpContext.Items["oUserCredential"] = oUserCredential;
                                break;
                            }
                        }
                    }
                    connection.Close();

                    if (pUser.app?.Trim().ToLower() != "mmu")
                        return "{'message': 'Bu login mmu uygulaması içindir.'}";

                    if (item.internall == -1 && item.externall == -1)
                        return "{'message': 'İsim ,parola veya cihaz id hatalı'}";

                    //if ((item.externall != -1 && item.external_iptal == "X") || (item.internall != -1 && item.internal_iptal == "X"))
                    //    return "{'message':'Hesap askıya alınmıştır. Lütfen yöneticiniz ile görüşünüz.'}";

                    if (item.external_iptal || item.internal_iptal == "1")
                        return "{'message': 'Kullanıcı bulunamadı.'}";

                    if (pUser.username != "APPSTORE" && pUser.username != "1" && pUser.username != "MEHMETUN")
                        if (!IsValidEmail(item.email))
                            return "{'message': 'Lütfen email adresinizi kontrol ediniz.'}";
                }
                return "";
            }
            catch
            {
                return "{'message': 'İsim ,parola veya cihaz id hatalı'}";
            }
        }

        public static bool IsValidEmail(string source)
        {
            return new EmailAddressAttribute().IsValid(source);
        }
    }
}
