using System.Collections.Generic;
using System.Data.SqlClient;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.IsTakip;

namespace MusteriMobilUygulamaAPI.Services.IsTakip
{
    public static class sSecimli
    {
        public static cResponseModel<cSecimli> get(string kod, string imei, string tip, string gumruk, string is_takip_kod)
        {
            //int y=0, x = 1/y;
            cSecimli oSecimli = new cSecimli();
            cResponseModel<cSecimli> oResponseModel = new cResponseModel<cSecimli>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            SqlConnection connection = new SqlConnection(builder.ConnectionString);

            connection.Open();

            List<cSecimli> data = new List<cSecimli>();

            string sql = @" exec [Evrim_DB].[dbo].[eistakipmobil_secimli] '" + kod + "', '" + imei + "', '" + tip + "', '" + gumruk + "', '" + is_takip_kod + "'";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cSecimli item = new cSecimli();
                        item.Dosya_No = reader.GetString(0);
                        item.Gonderen_tamunvan = reader.GetString(1);

                        data.Add(item);
                    }
                }
            }
            connection.Close();

            oResponseModel.Data = data;
            return oResponseModel;
        }
    }
}
