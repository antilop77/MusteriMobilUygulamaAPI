using System.Data.SqlClient;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.IsTakip;

namespace MusteriMobilUygulamaAPI.Services.IsTakip
{
    public static class sDilekceNoGetir
    {
        public static cResponseModel<cDilekceNoGetirOutput> get(cDilekceNoGetirInput param)
        {
            //int y=0, x = 1/y;
            List<cDilekceNoGetirOutput> items = new List<cDilekceNoGetirOutput>();
            cResponseModel<cDilekceNoGetirOutput> oResponseModel = new cResponseModel<cDilekceNoGetirOutput>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = @" exec [UGM_ERP].[dbo].[pGetDilekceTakipDilekceNoGetir] '" + param.dosyaNo + "','" + param.tip + "'";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cDilekceNoGetirOutput item = new cDilekceNoGetirOutput();
                            item.id = reader.GetInt32(0);
                            item.dilekceNoKonu = reader.GetString(1);

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
