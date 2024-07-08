using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models.ParaTalep;
using MusteriMobilUygulamaAPI.Models;
using System.Data.SqlClient;
using System.Data;

namespace MusteriMobilUygulamaAPI.Services.ParaTalep
{
    public class sParaTalepCezaOnay
    {
        public static cResponseModel<cParaTalepCezaOnay> getParaTalepCezaOnay()
        {
            List<cParaTalepCezaOnay> items = new List<cParaTalepCezaOnay>();
            cResponseModel<cParaTalepCezaOnay> oResponseModel = new cResponseModel<cParaTalepCezaOnay>();
            oResponseModel.Data = items;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = $@" exec [UGM_DB].[dbo].EVR_ParaTalepCezaOnay ''";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        items = dataTable.AsEnumerable().Select(m => new cParaTalepCezaOnay()
                        {
                            Detay_No= m.Field<int>("Detay_No"),
                            IsiVeren= m.Field<string>("IsiVeren"),
                            Tip = m.Field<string>("Tip"),
                            ReferansNo = m.Field<string>("ReferansNo"),
                            Musteri = m.Field<string>("Musteri"),
                            GirisTarih = m.Field<DateTime?>("GirisTarih"),
                            GirisSaati = m.Field<string>("GirisSaati"),
                            Tutar = m.Field<double?>("Tutar"),
                            Doviz = m.Field<string>("Doviz"),
                            Cins = m.Field<string>("Cins"),
                            CinsAciklama = m.Field<string>("CinsAciklama"),
                            ParayiAlanHesapNo = m.Field<string>("ParayiAlanHesapNo"),
                            HesapAdi = m.Field<string>("HesapAdi"),
                            Kayit_tarihi = m.Field<string>("Kayit_tarihi"),
                            MC = m.Field<string>("MC"),
                            UnspedOnayKulKod = m.Field<string>("UnspedOnayKulKod"),
                            Bakiye = m.Field<double?>("Bakiye"),
                            UnspedOnay = m.Field<bool?>("UnspedOnay"),
                            UnspedOnayTarih = m.Field<DateTime?>("UnspedOnayTarih"),
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
