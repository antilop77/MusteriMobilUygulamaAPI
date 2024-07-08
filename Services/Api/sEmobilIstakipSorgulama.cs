using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MmuAPI.Models;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.Api;

namespace MusteriMobilUygulamaAPI.Services.Api
{
    public class sEmobilIstakipSorgulama
    {
        public static cResponseModel<cEmobilIstakipSorgulama>? getEmobilIstakipSorgulama(HttpContext httpContext, string firma_no, string referans, string referans_tipi, string tip, string kullanici, string parola)
        {
            cResponseModel<cEmobilIstakipSorgulama> oResponseModel = new cResponseModel<cEmobilIstakipSorgulama>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            List<cEmobilIstakipSorgulama> items = new List<cEmobilIstakipSorgulama>();

            cUserCredential oUserCredential = new cUserCredential();
            httpContext.Items.TryGetValue("oUserCredential", out var x);
            oUserCredential = (cUserCredential)x;

            //int? user_id = oUserCredential?.in_user_id;
            //int? ex_user_id = oUserCredential?.ex_user_id;
            string? UDID = oUserCredential?.imei;

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            string sql = $@" exec Evrim_DB.dbo.emobil_IstakipSorgulama {firma_no}, '{referans}', '{referans_tipi}', '{tip}', '{kullanici}', '{parola}', '{UDID}' ";

            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader readerForLikes = command.ExecuteReader();
            DataTable dataTableForLikes = new DataTable();
            dataTableForLikes.Load(readerForLikes);
            connection.Close();
                             
            if (dataTableForLikes?.Rows.Count > 0 && dataTableForLikes?.Rows[0]?.ItemArray[0]?.ToString() == "GİRİŞ YETKİNİZ BULUNMAMAKTA")
            {
                oResponseModel.errorModel.ErrorCode = -1;
                oResponseModel.errorModel.ErrorMessage = "GİRİŞ YETKİNİZ BULUNMAMAKTA";
                return oResponseModel;
            }
            dataTableForLikes = dataTableForLikes == null ? new DataTable() : dataTableForLikes;

            foreach (DataRow row in dataTableForLikes.Rows)
            {
                    cEmobilIstakipSorgulama item = new cEmobilIstakipSorgulama();
                                        
                    item.SiraNo = row["SiraNo"] == DBNull.Value ? "" : row["SiraNo"].ToString();
                    item.Dosya_no = row["Dosya_no"] == DBNull.Value ? "" : row["Dosya_no"].ToString();
                    item.Referans = row["Referans"] == DBNull.Value ? "" : row["Referans"].ToString();
                    item.Alici = row["Alici"] == DBNull.Value ? "" : row["Alici"].ToString();
                    item.Kur_Tarihi = row["Kur_Tarihi"] == DBNull.Value ? "" : row["Kur_Tarihi"].ToString();
                    item.FaturaBedeli = row["FaturaBedeli"] == DBNull.Value ? "" : row["FaturaBedeli"].ToString();
                    item.Doviz_kodu = row["Doviz_kodu"] == DBNull.Value ? "" : row["Doviz_kodu"].ToString();
                    item.Miktar = row["Miktar"] == DBNull.Value ? "" : row["Miktar"].ToString();
                    item.BrutKg = row["BrutKg"] == DBNull.Value ? "" : row["BrutKg"].ToString();
                    item.gumruk = row["gumruk"] == DBNull.Value ? "" : row["gumruk"].ToString();
                    item.KapAdedi = row["KapAdedi"] == DBNull.Value ? "" : row["KapAdedi"].ToString();
                    item.BulunduguYer = row["BulunduguYer"] == DBNull.Value ? "" : row["BulunduguYer"].ToString();
                    item.Nakliyeci = row["Nakliyeci"] == DBNull.Value ? "" : row["Nakliyeci"].ToString();
                    item.Aciklama = row["Aciklama"] == DBNull.Value ? "" : row["Aciklama"].ToString();
                    item.Tarihsaat = row["Tarihsaat"] == DBNull.Value ? "" : row["Tarihsaat"].ToString();

                    items.Add(item);
            }
            oResponseModel.Data = items;                            
            return oResponseModel;
        }
    }
}
