using MusteriMobilUygulamaAPI.Models.Api;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Services.Api;
using System.Data.SqlClient;
using System.Data;
using MusteriMobilUygulamaAPI.Models.Intranet;
using System.Numerics;

namespace MusteriMobilUygulamaAPI.Services.Intranet
{
    public class sKuryeRingSaatleri
    {
        public static cResponseModel<cKuryeRingSaatleri> getKuryeRingSaatleri()
        {
            List<cKuryeRingSaatleri> items = new List<cKuryeRingSaatleri>();
            cResponseModel<cKuryeRingSaatleri> oResponseModel = new cResponseModel<cKuryeRingSaatleri>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionStringForKurumsal;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = $@" exec Intranet.dbo.KuryeRingSaatleri ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        items = dataTable.AsEnumerable().Select(m => new cKuryeRingSaatleri()
                        {
                            Id = m.Field<int>("Id"),
                            GumrukOfisi = m.Field<string?>("GumrukOfisi"),
                            RingSaati = m.Field<string?>("RingSaati")                            
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
