using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MmuAPI.Models;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.Api;
using MusteriMobilUygulamaAPI.Models.IsTakip;
using MusteriMobilUygulamaAPI.Services.IsTakip;

namespace MusteriMobilUygulamaAPI.Services.Api
{
    public class sSavunmaGirilmesiGerekenKodlar
    {
        public static cResponseModel<cSavunmaGirilmesiGerekenKodlar>? getSavunmaGirilmesiGerekenKodlar(cUserCredential oUserCredential)
        {
            cResponseModel<cSavunmaGirilmesiGerekenKodlar> oResponseModel = new cResponseModel<cSavunmaGirilmesiGerekenKodlar>();
            if (oUserCredential.in_user_id == -1)
            {
                oResponseModel.errorModel.ErrorCode = -1;
                oResponseModel.errorModel.ErrorMessage = "Yetki sorunu oluştu.";
                return oResponseModel;
            }

            List<cUgmUser>? oUgmUsers = oUserCredential.in_user_id == null ? new List<cUgmUser>() : cCommon.getUgmUser((int)oUserCredential.in_user_id);

            if (oUgmUsers?.Count == 0)
            {
                return oResponseModel;
            }
                
            string userName = oUgmUsers?[0].Kodu;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            List<cSavunmaGirilmesiGerekenKodlar> oSavunmaGirilmesiGerekenKodlarS = new List<cSavunmaGirilmesiGerekenKodlar>();

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            string sql = @" SELECT Kod
                                , KodGB
                                , Sure
                            FROM [UGM_ERP].[dbo].[SavunmaGirilecekIsTakipKodu] with(nolock)
                            WHERE 1=1
                            and ISNULL(Aktif,0) !=0 ";

            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);           
                         
            foreach (DataRow item in dataTable.Rows)
            {                
                string? Kod = item["Kod"] == DBNull.Value ? null : item["Kod"].ToString();
                string? KodGB = item["KodGB"] == DBNull.Value ? null : item["KodGB"].ToString();
                int? Sure = item["Sure"] == DBNull.Value ? null : (int)item["Sure"];

                
                //EXEC UGM_ERP.dbo.pGetKullanicilarinSavunmaGirecegiIsTakipKodlari 'T','{Kod}','{KodGb}',{Sure},'{KulKod}'
                
                sql = @" exec [UGM_ERP].[dbo].[pGetKullanicilarinSavunmaGirecegiIsTakipKodlari] 'T', '" + Kod + @"'
                                                                                                          , '" + KodGB + @"'
                                                                                                          , " + Sure + ", '" + userName + "'";

                using (command = new SqlCommand(sql, connection))
                {
                    using (reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cSavunmaGirilmesiGerekenKodlar oSavunmaGirilmesiGerekenKodlar = new cSavunmaGirilmesiGerekenKodlar();
                            oSavunmaGirilmesiGerekenKodlar.IsTakipT_ID = reader["IsTakipT_ID"] == DBNull.Value ? null : reader["IsTakipT_ID"].ToString();
                            oSavunmaGirilmesiGerekenKodlar.Dosya_No = reader["Dosya_No"] == DBNull.Value ? null : reader["Dosya_No"].ToString();
                            oSavunmaGirilmesiGerekenKodlar.Tip = reader["Tip"] == DBNull.Value ? null : reader["Tip"].ToString();
                            oSavunmaGirilmesiGerekenKodlar.KulKod = reader["KulKod"] == DBNull.Value ? null : reader["KulKod"].ToString();
                            oSavunmaGirilmesiGerekenKodlar.KodGB = reader["KodGB"] == DBNull.Value ? null : reader["KodGB"].ToString();
                            oSavunmaGirilmesiGerekenKodlar.KodGB_TarihSaat = reader["KodGB_TarihSaat"] == DBNull.Value ? null : ((DateTime)reader["KodGB_TarihSaat"]).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                            oSavunmaGirilmesiGerekenKodlar.Kod = reader["Kod"] == DBNull.Value ? null : reader["Kod"].ToString();
                            oSavunmaGirilmesiGerekenKodlar.Kod_TarihSaat = reader["Kod_TarihSaat"] == DBNull.Value ? null : ((DateTime)reader["Kod_TarihSaat"]).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                            oSavunmaGirilmesiGerekenKodlar.Fark = reader["Fark"] == DBNull.Value ? null : reader["Fark"].ToString();
                            oSavunmaGirilmesiGerekenKodlar.KulNot = reader["KulNot"] == DBNull.Value ? null : reader["KulNot"].ToString();

                            oSavunmaGirilmesiGerekenKodlarS.Add(oSavunmaGirilmesiGerekenKodlar);
                        }
                    }
                }
            }
            connection.Close();
            
            oResponseModel.Data = oSavunmaGirilmesiGerekenKodlarS;                            
            return oResponseModel;
        }

        public static cResponseModel<int>? postSavunmaGirilmesiGerekenKodlar(string pIsTakipT_ID, string pKulNot)
        {
            cResponseModel<int> oResponseModel = new cResponseModel<int>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            
            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();
                
            string sql = $@"Update Evrim_Db.dbo.IsTakipT Set KulNot='{pKulNot}' where IsTakipT_ID={pIsTakipT_ID}";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                int x = command.ExecuteNonQuery();
                if (x > 0)
                {
                    oResponseModel.errorModel.ErrorMessage = "islem tamam";
                }
                else
                {
                    oResponseModel.errorModel.ErrorCode = -1;
                    oResponseModel.errorModel.ErrorMessage = "islem basarisiz";
                }
            }
            connection.Close();
                                      
            return oResponseModel;
        }
    }
}
