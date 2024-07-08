using MusteriMobilUygulamaAPI.Models.Api;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Services.Api;
using System.Data.SqlClient;
using System.Data;
using MusteriMobilUygulamaAPI.Models.ParaTalep;

namespace MusteriMobilUygulamaAPI.Services.ParaTalep
{
    public class sParaTalepOnayListesi
    {
        public static cResponseModel<cParaTalepOnayListesi> getParaTalepOnayListesi(DateTime baslangicTarihi, DateTime bitisTarihi)
        {
            List<cParaTalepOnayListesi> items = new List<cParaTalepOnayListesi>();
            cResponseModel<cParaTalepOnayListesi> oResponseModel = new cResponseModel<cParaTalepOnayListesi>();
            oResponseModel.Data = items;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = $@" Set Dateformat dmy  Select p.ID, P.Detay_No, P.Tip, P.ReferansNo, P.Musteri,E.Tanim, P.Tutar,  D.Doviz_Kodu
                        From Paratalep P
                        left outer join  Evraklar E on  P.MC=e.Kod  
                        left outer join Doviz D on P.DovizCinsi = d.Sira_No                                                        
                        Where  GirisTarih >= '{baslangicTarihi.ToString("dd.MM.yyyy")}'
                        and GirisTarih <= '{bitisTarihi.ToString("dd.MM.yyyy")}'
                        and (p.OnayVeren IS NULL OR p.OnayVeren = '')  
                        and (P.Sirket_No like '%M' or IsNull(P.Sirket_No,'')='')
                        Order by Detay_No desc ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        items = dataTable.AsEnumerable().Select(m => new cParaTalepOnayListesi()
                        {
                            ID = m.Field<int>("ID"),
                            Detay_No = m.Field<int?>("Detay_No"),
                            Tip = m.Field<string?>("Tip"),
                            ReferansNo = m.Field<string?>("ReferansNo"),
                            Musteri = m.Field<string?>("Musteri"),
                            Tanim = m.Field<string?>("Tanim"),
                            Tutar = m.Field<double?>("Tutar"),
                            Doviz_Kodu = m.Field<string?>("Doviz_Kodu"),
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
