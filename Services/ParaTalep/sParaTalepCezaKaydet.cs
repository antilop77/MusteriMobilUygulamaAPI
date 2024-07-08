using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models.ParaTalep;
using MusteriMobilUygulamaAPI.Models;
using System.Data.SqlClient;
using System.Data;

namespace MusteriMobilUygulamaAPI.Services.ParaTalep
{
    public class sParaTalepCezaKaydet
    {
        public static cResponseModel<int?> postParaTalepCezaKaydet(int detayNo, string onayRed, string isiVeren, string kullanici_kodu, string onayNot)
        {
            cResponseModel<int?> oResponseModel = new cResponseModel<int?>();
            oResponseModel.Data = null;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = $@"exec [UGM_DB].[dbo].EVR_SaveParaTalepCezaOnay '{onayRed}', {detayNo}, '{isiVeren}', '{kullanici_kodu}',	'{onayNot}'";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                    }
                }
                connection.Close();
            }

            return oResponseModel;
        }
    }
}