using System.Data.SqlClient;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.IsTakip;

namespace MusteriMobilUygulamaAPI.Services.IsTakip
{
    public static class sSonIslemler
    {
        public static cResponseModel<cSonIslemler> get(string kod)
        {
            //int y=0, x = 1/y;
            List<cSonIslemler> items = new List<cSonIslemler>();
            cResponseModel<cSonIslemler> oResponseModel = new cResponseModel<cSonIslemler>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = @" exec [Evrim_DB].[dbo].[eistakipmobil_sonislemler] '" + kod + "', ''";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cSonIslemler item = new cSonIslemler();
                            item.Kod = reader.GetString(0);
                            item.Aciklama = reader.GetString(1);

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
