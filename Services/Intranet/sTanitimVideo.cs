using MusteriMobilUygulamaAPI.Models.Api;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Services.Api;
using System.Data.SqlClient;
using System.Data;
using MusteriMobilUygulamaAPI.Models.Intranet;

namespace MusteriMobilUygulamaAPI.Services.Intranet
{
    public class sTanitimVideo
    {
        public static cResponseModel<cTanitimVideo> getTanitimVideo()
        {
            List<cTanitimVideo> items = new List<cTanitimVideo>();
            cResponseModel<cTanitimVideo> oResponseModel = new cResponseModel<cTanitimVideo>();
            
            oResponseModel.Data = items;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionStringForKurumsal;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = $@" SELECT VideoBaslik,Link FROM [Intranet].[dbo].[TanitimVideo] ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        items = dataTable.AsEnumerable().Select(m => new cTanitimVideo()
                        {
                            VideoBaslik = m.Field<string?>("VideoBaslik"),
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
