using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.Intranet;
using System.Data.SqlClient;

namespace MusteriMobilUygulamaAPI.Services.Intranet
{
    public class sIzinDuyurulari
    {
        public static cResponseModel<cIzinDuyurulari> getIzinDuyurulari()
        {            
            List<cIzinDuyurulari> items = new List<cIzinDuyurulari>();
            cResponseModel<cIzinDuyurulari> oResponseModel = new cResponseModel<cIzinDuyurulari>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionStringForIdari;
            
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                
                string sql = $@" exec Idari.dbo.IntranetKurumsalIletisimDuyurulari 11 ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cIzinDuyurulari item = new cIzinDuyurulari();
                            item.adsoyadi = reader["adsoyadi"].ToString();
                            item.tariharaligi = reader["unvan"].ToString();
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
