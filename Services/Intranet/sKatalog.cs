using MusteriMobilUygulamaAPI.Models.Api;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Services.Api;
using System.Data.SqlClient;
using System.Data;
using MusteriMobilUygulamaAPI.Models.Intranet;

namespace MusteriMobilUygulamaAPI.Services.Intranet
{
    public class sKatalog
    {
        public static cResponseModel<cKatalog> getKatalog()
        {
            List<cKatalog> items = new List<cKatalog>();
            cResponseModel<cKatalog> oResponseModel = new cResponseModel<cKatalog>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionStringForKurumsal;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = $@" SELECT RTRIM(REPLACE(REPLACE(KatalogAdi,'-',''),KatalogYil,'')) AS KatalogAdi,KatalogLink FROM [Intranet].[dbo].[Katalog] WHERE KatalogYil = (SELECT MAX(KatalogYil) FROM [Intranet].[dbo].[Katalog]) ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        items = dataTable.AsEnumerable().Select(m => new cKatalog()
                        {
                            KatalogAdi = m.Field<string?>("KatalogAdi"),
                            KatalogLink = m.Field<string?>("KatalogLink")
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
