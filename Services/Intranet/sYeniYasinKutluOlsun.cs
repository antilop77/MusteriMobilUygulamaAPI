using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.Intranet;
using System.Data.SqlClient;

namespace MusteriMobilUygulamaAPI.Services.Intranet
{
    public class sYeniYasinKutluOlsun
    {
        public static cResponseModel<cYeniYasinKutluOlsun> getYeniYasinKutluOlsun()
        {            
            List<cYeniYasinKutluOlsun> items = new List<cYeniYasinKutluOlsun>();
            cResponseModel<cYeniYasinKutluOlsun> oResponseModel = new cResponseModel<cYeniYasinKutluOlsun>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionStringForKurumsal;
            
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                
                string sql = $@" exec Intranet.dbo.BugunDoganPersonel ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cYeniYasinKutluOlsun item = new cYeniYasinKutluOlsun();
                            item.Adi_Soyadi = reader["Adi_Soyadi"].ToString();
                            item.DepartmanAdi = reader["DepartmanAdi"].ToString();
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
