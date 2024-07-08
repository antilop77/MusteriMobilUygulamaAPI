using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using MusteriMobilUygulamaAPI.Models.IsTakip;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;

namespace MusteriMobilUygulamaAPI.Services.IsTakip
{
    public static class sKullaniciDosyalari
    {
        public static cResponseModel<cKullaniciDosyalari> get(string kullanici)
        {
            //int y=0, x = 1/y;
            List<cKullaniciDosyalari> items = new List<cKullaniciDosyalari>();
            cResponseModel<cKullaniciDosyalari> oResponseModel = new cResponseModel<cKullaniciDosyalari>();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = @" select DosyaNo, a.Kullanici, TarihSaat, a.TakipKodu "
                            + " , (select Aciklama from [Evrim_DB].[dbo].[IsTakip] it where it.kod = a.TakipKodu and it.Mobil='X') aciklama "
                            + " from [Evrim_DB].[dbo].[emobilIsTakip_Giris] a with(nolock) "
                            + " where 1=1 "
                            + " and TarihSaat >= cast(GETDATE()-5 as date) "
                            + " and a.Kullanici = '" + kullanici + "' ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cKullaniciDosyalari item = new cKullaniciDosyalari();
                            item.DosyaNo = reader.GetString(0);
                            item.Kullanici = reader.GetString(1);
                            item.TarihSaat = reader.GetDateTime(2);
                            item.aciklama = reader.GetString(3);

                            items.Add(item);
                        }
                    }
                }
                connection.Close();
            }
            oResponseModel.Data = items;
            return oResponseModel;
        }
    }
}
