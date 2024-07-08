using System.Data.SqlClient;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.IsTakip;

namespace MusteriMobilUygulamaAPI.Services.IsTakip
{
    public static class sMobilAppVersion
    {
        public static cResponseModel<cMobilAppVersion> get(string application_name)
        {
            List<cMobilAppVersion> items = new List<cMobilAppVersion>();
            cResponseModel<cMobilAppVersion> oResponseModel = new cResponseModel<cMobilAppVersion>();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = @" select Version, Note, ReleaseDate "
                            + " from [Evrim_DB].[dbo].[MobilAppVersion] "
                            + " where 1=1 "
                            + " and ApplicationName = '" + application_name + "' ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cMobilAppVersion item = new cMobilAppVersion();
                            item.Version = reader.GetString(0);
                            item.Note = reader.GetString(1);
                            item.ReleaseDate = reader.GetDateTime(2);

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
