using MusteriMobilUygulamaAPI.Models.Api;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Services.Api;
using System.Data.SqlClient;
using System.Data;
using MusteriMobilUygulamaAPI.Models.Intranet;

namespace MusteriMobilUygulamaAPI.Services.Intranet
{
    public class sKurumsalDerecelendirme
    {
        public static cResponseModel<cKurumsalDerecelendirme> getKurumsalDerecelendirme()
        {
            List<cKurumsalDerecelendirme> items = new List<cKurumsalDerecelendirme>();
            cResponseModel<cKurumsalDerecelendirme> oResponseModel = new cResponseModel<cKurumsalDerecelendirme>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionStringForKurumsal;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = $@" SELECT LTRIM(REPLACE(KurumsalBaslik,KurumsalYil,'')) as KurumsalBaslik ,Link FROM [Intranet].[dbo].[KurumsalDerecelendirme] WHERE KurumsalYil=(SELECT MAX(KurumsalYil) FROM [Intranet].[dbo].[KurumsalDerecelendirme]) ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        items = dataTable.AsEnumerable().Select(m => new cKurumsalDerecelendirme()
                        {
                            KurumsalBaslik = m.Field<string?>("KurumsalBaslik"),
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
