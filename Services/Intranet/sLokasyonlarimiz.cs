using MusteriMobilUygulamaAPI.Models.Api;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Services.Api;
using System.Data.SqlClient;
using System.Data;
using MusteriMobilUygulamaAPI.Models.Intranet;

namespace MusteriMobilUygulamaAPI.Services.Intranet
{
    public class sLokasyonlarimiz
    {
        public static cResponseModel<cLokasyonlarimiz> getLokasyonlarimiz()
        {
            List<cLokasyonlarimiz> items = new List<cLokasyonlarimiz>();
            cResponseModel<cLokasyonlarimiz> oResponseModel = new cResponseModel<cLokasyonlarimiz>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = $@" exec Evrim_DB.dbo.SubeListesi  ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        items = dataTable.AsEnumerable().Select(m => new cLokasyonlarimiz()
                        {
                            ID = m.Field<int>("ID"),
                            OfisAdi = m.Field<string?>("OfisAdi"),
                            Sehir = m.Field<string?>("Sehir"),
                            Telefon1 = m.Field<string?>("Telefon1"),
                            Telefon2 = m.Field<string?>("Telefon2"),
                            Fax1 = m.Field<string?>("Fax1"),
                            Fax2 = m.Field<string?>("Fax2"),
                            Yetkili = m.Field<string?>("Yetkili"),
                            Koordinat = m.Field<string?>("Koordinat"),
                            Dil = m.Field<string?>("Dil"),
                            Adres = m.Field<string?>("Adres")
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
