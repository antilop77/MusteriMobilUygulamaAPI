using System;
using System.Data.SqlClient;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.IsTakip;

namespace MusteriMobilUygulamaAPI.Services.IsTakip
{
    public static class sDosyaTamUrunBildirimleri
    {
        public static cResponseModel<cDosyaTamUrunBildirimleri> get(DateTime baslangic_tarihi, DateTime bitis_tarihi)
        {
            //int y=0, x = 1/y;
            List<cDosyaTamUrunBildirimleri> items = new List<cDosyaTamUrunBildirimleri>();
            cResponseModel<cDosyaTamUrunBildirimleri> oResponseModel = new cResponseModel<cDosyaTamUrunBildirimleri>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = $@" select DosyaNo as dosya_no ,OnBildirimNo as on_bildirim_no
                                from UGM_ERP.dbo.Tarim_GGBS_Sorgulama
                                where 1=1
                                and DosyaNo in (select Dosya_No 
				                                from Evrim_DB.dbo.Istakipt
				                                where 1=1
				                                and kod = 'K03' 
				                                and TarihSaat between '{baslangic_tarihi.ToString("dd.MM.yyyy 00:00:00")}' and '{bitis_tarihi.ToString("dd.MM.yyyy 23:59:59")}'
				                                and Tip = 'T' ) 
                                and IslemYeri = 'İSTANBUL' order by DosyaNo, OnBildirimNo ";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cDosyaTamUrunBildirimleri item = new cDosyaTamUrunBildirimleri();
                            item.dosya_no = reader.GetString(0);
                            item.on_bildirim_no = reader.GetString(1);

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
