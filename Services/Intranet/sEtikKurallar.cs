using MusteriMobilUygulamaAPI.Models.Api;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Services.Api;
using System.Data.SqlClient;
using System.Data;
using MusteriMobilUygulamaAPI.Models.Intranet;

namespace MusteriMobilUygulamaAPI.Services.Intranet
{
    public class sEtikKurallar
    {
        public static cResponseModel<cEtikKurallar> getEtikKurallar()
        {
            List<cEtikKurallar> items = new List<cEtikKurallar>();
            cResponseModel<cEtikKurallar> oResponseModel = new cResponseModel<cEtikKurallar>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionStringForKurumsal;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = $@" SELECT [Baslik],[Link] FROM [Intranet].[dbo].[EtikKurallar] ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        items = dataTable.AsEnumerable().Select(m => new cEtikKurallar()
                        {
                            Baslik = m.Field<string?>("Baslik"),
                            Link = m.Field<string?>("Link")
                        }).ToList();
                    }
                }
                connection.Close();
            }

            oResponseModel.Data = items;
            return oResponseModel;
        }
    }
}
