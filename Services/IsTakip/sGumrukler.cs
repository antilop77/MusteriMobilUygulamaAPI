using System.Data.SqlClient;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.IsTakip;

namespace MusteriMobilUygulamaAPI.Services.IsTakip
{
    public static class sGumrukler
    {
        public static cResponseModel<cGumrukler> get()
        {
            //int y=0, x = 1/y;
            List<cGumrukler> items = new List<cGumrukler>();
            cResponseModel<cGumrukler> oResponseModel = new cResponseModel<cGumrukler>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = @" select [Gumruk_No]
                                    , [Gumruk]
                                    , [Gumruk_Kodu]
                                from [Evrim_DB].[dbo].[Gumruk] ";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cGumrukler item = new cGumrukler();
                            item.gumruk_no = reader.GetInt16(0);
                            item.gumruk = reader["Gumruk"] == DBNull.Value ? null : reader.GetString(1);
                            item.gumruk_kodu = reader["Gumruk_Kodu"] == DBNull.Value ? null : reader.GetString(2);

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
