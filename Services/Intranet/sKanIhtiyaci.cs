using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.Intranet;
using System.Data.SqlClient;

namespace MusteriMobilUygulamaAPI.Services.Intranet
{
    public class sKanIhtiyaci
    {
        public static cResponseModel<cKanIhtiyaci> getKanIhtiyaci()
        {            
            List<cKanIhtiyaci> items = new List<cKanIhtiyaci>();
            cResponseModel<cKanIhtiyaci> oResponseModel = new cResponseModel<cKanIhtiyaci>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionStringForIdari;
            
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                
                string sql = $@" exec Idari.dbo.IntranetKurumsalIletisimDuyurulari 6 ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cKanIhtiyaci item = new cKanIhtiyaci();
                            item.AdSoyadi = reader["AdSoyadi"].ToString();
                            item.Tarih = reader["Tarih"].ToString();
                            item.Unvan = reader["Unvan"].ToString();
                            item.URL = reader["URL"].ToString();
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
