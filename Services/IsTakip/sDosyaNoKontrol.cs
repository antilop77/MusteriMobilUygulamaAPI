using System;
using System.Data.SqlClient;
using System.Reflection;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.IsTakip;

namespace MusteriMobilUygulamaAPI.Services.IsTakip
{
    public static class sDosyaNoKontrol
    {
        public static cResponseModel<cDosyaNoKontrolOutput> get(cDosyaNoKontrolInput param)
        {
            //int y=0, x = 1/y;
            cResponseModel<cDosyaNoKontrolOutput> oResponseModel = new cResponseModel<cDosyaNoKontrolOutput>();

            List<cDosyaNoKontrolOutput> items = new List<cDosyaNoKontrolOutput>();

            string[] dosyaNumaralari = param.dosyaNo?.Split(',');

            //List<string> dosyaFormatMesaji = cCommon.DosyaNoValidation(dosyaNumaralari.ToList());
            List<cModel> modelList = new List<cModel>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                string sql = "";
                if (param.tip?.ToUpper().Trim() == "T")
                {
                    //modelList = db.Ggbmastr.Where(x => dosyaNumaralari.Contains(x.DosyaNo)).Select(x => new Model() { DosyaNo = x.DosyaNo, GonderenNo = x.GonderenNo }).ToList();
                    sql = @"select Dosya_No, isnull(Gonderen_No, 0) as Gonderen_No
                            from [Evrim_DB].[dbo].[GGBmastr]
                            where 1=1
                            and trim(Dosya_No) in (trim('" + param.dosyaNo + "')) ";
                }
                if (param.tip?.ToUpper().Trim() == "H")
                {
                    //modelList = db.Gcbmstr.Where(x => dosyaNumaralari.Contains(x.DosyaNo)).Select(x => new Model() { DosyaNo = x.DosyaNo, GonderenNo = x.GonderenNo }).ToList();
                    sql = @"select Dosya_No, isnull(GonderenNo, 0) as Gonderen_No
                            from [Evrim_DB].[dbo].[Gcbmstr]
                            where 1=1
                            and trim(Dosya_No) in (trim('" + param.dosyaNo + "')) ";
                }

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cDosyaNoKontrolOutput item = new cDosyaNoKontrolOutput();
                            item.dosyaNo = reader.GetString(0);
                            item.gonderenNo = reader.GetInt32(1);

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

