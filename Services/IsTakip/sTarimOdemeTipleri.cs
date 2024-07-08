using System.Data.SqlClient;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.IsTakip;

namespace MusteriMobilUygulamaAPI.Services.IsTakip
{
    public static class sTarimOdemeTipleri
    {
        public static cResponseModel<cTarimOdemeTipleri> get()
        {
            //int y=0, x = 1/y;
            List<cTarimOdemeTipleri> items = new List<cTarimOdemeTipleri>();
            cResponseModel<cTarimOdemeTipleri> oResponseModel = new cResponseModel<cTarimOdemeTipleri>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = @" select ID, OdemeTipi, Tip "
                            + " from [UGM_ERP].[dbo].[Tarim_Gida_OdemeTipleri] "
                            + " where 1=1 "
                            + " and Aktif = 1 ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cTarimOdemeTipleri item = new cTarimOdemeTipleri();
                            item.ID = reader.GetInt32(0);
                            item.OdemeTipi = reader.GetString(1);
                            item.Tip = reader.GetInt32(2);

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
