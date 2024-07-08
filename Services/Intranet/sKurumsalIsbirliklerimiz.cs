using MusteriMobilUygulamaAPI.Models.Api;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Services.Api;
using System.Data.SqlClient;
using System.Data;
using MusteriMobilUygulamaAPI.Models.Intranet;

namespace MusteriMobilUygulamaAPI.Services.Intranet
{
    public class sKurumsalIsbirliklerimiz
    {
        public static cResponseModel<cKurumsalIsbirliklerimiz> getKurumsalIsbirliklerimiz()
        {
            List<cKurumsalIsbirliklerimiz> items = new List<cKurumsalIsbirliklerimiz>();
            cResponseModel<cKurumsalIsbirliklerimiz> oResponseModel = new cResponseModel<cKurumsalIsbirliklerimiz>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionStringForKurumsal;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = $@" SELECT Firma,Konu,Indirim,Iletisim,Dokuman FROM [Intranet].[dbo].Kurumsalisbirligi ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        items = dataTable.AsEnumerable().Select(m => new cKurumsalIsbirliklerimiz()
                        {
                            Firma = m.Field<string?>("Firma"),
                            Konu = m.Field<string?>("Konu"),
                            Indirim = m.Field<string?>("Indirim"),
                            Iletisim = m.Field<string?>("Iletisim"),
                            Dokuman = m.Field<string?>("Dokuman")
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
