using MusteriMobilUygulamaAPI.Models.Api;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Services.Api;
using System.Data.SqlClient;
using System.Data;
using MusteriMobilUygulamaAPI.Models.Intranet;

namespace MusteriMobilUygulamaAPI.Services.Intranet
{
    public class sMaliDenetim
    {
        public static cResponseModel<cMaliDenetim> getMaliDenetim()
        {
            List<cMaliDenetim> items = new List<cMaliDenetim>();
            cResponseModel<cMaliDenetim> oResponseModel = new cResponseModel<cMaliDenetim>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionStringForKurumsal;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = $@" SELECT [DenetimBaslik],[Link] FROM [Intranet].[dbo].[MaliDenetim] ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        items = dataTable.AsEnumerable().Select(m => new cMaliDenetim()
                        {
                            DenetimBaslik = m.Field<string?>("DenetimBaslik"),
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
