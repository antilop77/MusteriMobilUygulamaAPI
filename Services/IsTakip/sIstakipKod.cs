using System.Data.SqlClient;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.IsTakip;

namespace MusteriMobilUygulamaAPI.Services.IsTakip
{
    public static class sIstakipKod
    {
        public static cResponseModel<cIsTakipKod> get()
        {
            //int y=0, x = 1/y;
            List<cIsTakipKod> items = new List<cIsTakipKod>();
            cResponseModel<cIsTakipKod> oResponseModel = new cResponseModel<cIsTakipKod>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = @" select isnull(kod, '') kod
    	                            ,isnull(Aciklama, '') Aciklama
                                    ,isnull(Tipler, '') Tipler
                                from [Evrim_DB].[dbo].[IsTakip] it 
                                where Mobil='X' ";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cIsTakipKod item = new cIsTakipKod();
                            item.Kod = reader.GetString(0);
                            item.Aciklama = reader.GetString(1);
                            item.Tipler = reader.GetString(2);

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
