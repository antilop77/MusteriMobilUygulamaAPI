using System.Data.SqlClient;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.IsTakip;

namespace MusteriMobilUygulamaAPI.Services.IsTakip
{
    public static class sGosterge
    {
        public static cResponseModel<cGosterge> get()
        {
            //int y=0, x = 1/y;
            List<cGosterge> items = new List<cGosterge>();
            cResponseModel<cGosterge> oResponseModel = new cResponseModel<cGosterge>();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = @" exec [Evrim_DB].[dbo].[eistakipmobil_gosterge2] '','',''";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cGosterge item = new cGosterge();
                            item.Kisi = reader.GetInt32(0);
                            item.Sube = reader.GetInt32(1);
                            item.Tum = reader.GetInt32(2);

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
