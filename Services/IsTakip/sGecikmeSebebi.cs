using System.Data.SqlClient;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.IsTakip;

namespace MusteriMobilUygulamaAPI.Services.IsTakip
{
    public static class sGecikmeSebebi
    {
        public static cResponseModel<cGecikmeSebebi> get()
        {
            //int y=0, x = 1/y;
            List<cGecikmeSebebi> items = new List<cGecikmeSebebi>();
            cResponseModel<cGecikmeSebebi> oResponseModel = new cResponseModel<cGecikmeSebebi>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = @" exec [Evrim_DB].[dbo].[eistakipmobil_gecikmesebebi] '', ''";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cGecikmeSebebi item = new cGecikmeSebebi();
                            item.Aciklama = reader.GetString(0);

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
