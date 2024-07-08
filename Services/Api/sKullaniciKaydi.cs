using System.Data;
using System.Data.SqlClient;
using System.Net;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.Api;
using static System.Net.WebRequestMethods;

namespace MusteriMobilUygulamaAPI.Services.Api
{
    public class sKullaniciKaydi
    {
        public static cResponseModel<int>? getKullaniciKaydi(HttpContext httpContext, string tc_no, string evrim_kodu, string ad_soyad, string email, string imei)
        {
            cResponseModel<int> oResponseModel = new cResponseModel<int>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            string sql = $@" SELECT *
                            FROM [Evrim_DB].[dbo].[Users]
                            WHERE TCKimlik='{tc_no}' and Kodu='{evrim_kodu}' and Mail='{email}' ";

            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            connection.Close();

            if (dataTable.Rows.Count == 0)
            {
                oResponseModel.errorModel.ErrorCode = -1;
                oResponseModel.errorModel.ErrorMessage = "İletilen bilgilerle eşleşen kullanıcı bulunamadı.";
                return oResponseModel;
            }
            
            string imeiOnDb = dataTable.Rows[0]["IMEI"] == DBNull.Value ? "" : dataTable?.Rows?[0]?["IMEI"]?.ToString()?.Trim();
            int users_id = dataTable?.Rows[0]["Users_ID"] == DBNull.Value ? 0 : Int32.Parse(dataTable?.Rows[0]["Users_ID"].ToString());            
            connection.Open();

            sql = $@" SELECT *
                            FROM [UGM_ERP].[dbo].[UBPAuthenticationMails]
                            WHERE EvrimKodu='{evrim_kodu}' and Address='{email}' and Validity=1 ";

            command = new SqlCommand(sql, connection);
            reader = command.ExecuteReader();
            dataTable = new DataTable();
            dataTable.Load(reader);
            connection.Close();            

            if (dataTable?.Rows.Count > 0)
            {
                int statusID = dataTable?.Rows[0]["StatusID"] == DBNull.Value ? 0 : Int32.Parse(dataTable?.Rows[0]["StatusID"].ToString());            

                if (statusID == 0)
                {
                    oResponseModel.errorModel.ErrorCode = -2;
                    oResponseModel.errorModel.ErrorMessage = "Bu kullanıcı için daha önce doğrulama e-postası gönderildi. Onayı bekleniyor.";
                    return oResponseModel;
                }
            }
            

            connection.Open();

            string auth_key = Guid.NewGuid().ToString();

            sql = $@" INSERT INTO [UGM_ERP].[dbo].[UBPAuthenticationMails] (TCNo, EvrimKodu, Address, AuthKey, StatusID) 
                        values ('{tc_no}', '{evrim_kodu}', '{email}', '{auth_key}', 0)";

            command = new SqlCommand(sql, connection);
            int cntInserted = command.ExecuteNonQuery();
           
            sql = $@" UPDATE [Evrim_DB].[dbo].[Users] SET IMEI='{imei}', Mobil='0' WHERE Users_ID={users_id}";

            command = new SqlCommand(sql, connection);
            int cntUpdated = command.ExecuteNonQuery();
            connection.Close();

            //string auth_url = "https://ubp.ugm.com.tr:9250" + $@"/api/MailAuth?evrim_kodu={evrim_kodu}&auth_key={auth_key}";
            //string auth_url = cCommon.UrlForApi_MailAuth + $@"/api/MailAuth?evrim_kodu={evrim_kodu}&auth_key={auth_key}";
            string auth_url = httpContext.Request.Scheme + "://" + httpContext.Request.Host.Value.ToString() + $@"/api/MailAuth?evrim_kodu={evrim_kodu}&auth_key={auth_key}";


            string mail_subject = "Mobil Uygulama Aktivasyonu";
            string ad_soyad_proper_case = ad_soyad;
            string mail_body = $@"Sayın {ad_soyad_proper_case},<br/>
                Mobil uygulama aktivasyon talebinizi tamamlamak için lütfen aşağıdaki bağlantıya tıklayınız:<br/>
                <a target=' ' href='{auth_url}'>{auth_url}</a>";   
            cSendMail oSendMail = new cSendMail();
            oSendMail.from = "bildirim1@ugm.com.tr";
            oSendMail.to = email;
            oSendMail.cc = "";
            oSendMail.bcc = "";
            oSendMail.subject = mail_subject;
            oSendMail.body = mail_body;
            oSendMail.displayName = "";

            MailProcess.SendMailBildirim(oSendMail);
            
            return oResponseModel;
        }

        public static cResponseModel<int>? getKullaniciSil(string userID)
        {
            cResponseModel<int> oResponseModel = new cResponseModel<int>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;


            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();
            
            string sql = $@"update [Evrim_DB].[dbo].[MobilKullanici] 
                            set Pasif = 1 
                            where 1=1 
                            and trim(kodu) = (select trim(kodu) 
                                              from Users 
                                              where 1=1 
                                              and users_Id = {userID}) ";

            SqlCommand command = new SqlCommand(sql, connection);
            int cntUpdated = command.ExecuteNonQuery();
            connection.Close();

            return oResponseModel;
        }
    

        public static cResponseModel<int>? getMailAuth(string evrim_kodu, string auth_key)
        {
            cResponseModel<int> oResponseModel = new cResponseModel<int>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();
            
            string sql = $@" SELECT ID, StatusID
                            FROM [UGM_ERP].[dbo].[UBPAuthenticationMails] 
                            WHERE 1=1
                            and Validity = 1 
                            and EvrimKodu = '{evrim_kodu}' 
                            and AuthKey = '{auth_key}' ";

            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            connection.Close();

            if (dataTable.Rows.Count == 0)
            {
                oResponseModel.errorModel.ErrorCode = -1;
                oResponseModel.errorModel.ErrorMessage = $"evrim_kodu : {evrim_kodu} ve auth_key : {auth_key} eşleşmedi.";
                return oResponseModel;
            }

            string statusId = dataTable.Rows[0]["StatusID"].ToString();
            string ID = dataTable.Rows[0]["ID"].ToString();

            if (statusId != "0")
            { 
                oResponseModel.errorModel.ErrorCode = -2;
                oResponseModel.errorModel.ErrorMessage = $"Girdiğiniz URL artık aktif değildir.";
                return oResponseModel;
            }
            
            connection.Open();

            
            sql = $@" UPDATE [UGM_ERP].[dbo].[UBPAuthenticationMails] SET StatusID='{statusId}' WHERE ID={ID} ";

            command = new SqlCommand(sql, connection);
            int cntInserted = command.ExecuteNonQuery();
           
            sql = $@" UPDATE [Evrim_DB].[dbo].[Users] SET Mobil='1' WHERE Kodu='{evrim_kodu}'";

            command = new SqlCommand(sql, connection);
            int cntUpdated = command.ExecuteNonQuery();
            connection.Close();
            
            oResponseModel.errorModel.ErrorMessage = "Aktivasyon başarıyla tamamlandı.";
            return oResponseModel;
        }
    }
}
