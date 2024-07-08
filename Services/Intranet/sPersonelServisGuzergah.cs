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
    public class sPersonelServisGuzergah
    {
        public static cResponseModel<cPersonelServisGuzergah> getPersonelServisGuzergah()
        {
            List<cPersonelServisGuzergah> items = new List<cPersonelServisGuzergah>();
            cResponseModel<cPersonelServisGuzergah> oResponseModel = new cResponseModel<cPersonelServisGuzergah>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionStringForKurumsal;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = $@" exec Intranet.dbo.PersonelServisGuzergah ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        items = dataTable.AsEnumerable().Select(m => new cPersonelServisGuzergah()
                        {
                            AracNo = m.Field<int>("AracNo"),
                            Plaka = m.Field<string?>("Plaka"),
                            Sahibi = m.Field<string?>("Sahibi"),
                            Telefon = m.Field<string?>("Telefon"),
                            GuzergahAdi = m.Field<string?>("GuzergahAdi"),
                            GuzergahKonum = m.Field<string?>("GuzergahKonum"),
                            GuzergahId = m.Field<int>("GuzergahId"),
                            
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
