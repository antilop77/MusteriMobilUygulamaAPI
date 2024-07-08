using System.Data.SqlClient;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.IsTakip;

namespace MusteriMobilUygulamaAPI.Services.IsTakip
{
    public static class sKodTarih
    {
        public static cResponseModel<cKodTarih> get(string dosya_no, string dosya_tip)
        {
            //int y=0, x = 1/y;
            List<cKodTarih> items = new List<cKodTarih>();
            cResponseModel<cKodTarih> oResponseModel = new cResponseModel<cKodTarih>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = @" exec [Evrim_DB].[dbo].[eistakipmobil_kodtarih] '', '" + dosya_no + "','" + dosya_tip + "'";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cKodTarih item = new cKodTarih();
                            item.IlkTarih = reader.GetDateTime(0);
                            item.SonTarih = reader.GetDateTime(1);

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
