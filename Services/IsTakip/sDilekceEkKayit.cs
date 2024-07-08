using System;
using System.Data.SqlClient;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.IsTakip;

namespace MusteriMobilUygulamaAPI.Services.IsTakip
{
    public static class sDilekceEkKayit
    {
        public static cResponseModel<cDilekceEkKayitResult> get(cDilekceEkKayit param)
        {
            //int y=0, x = 1/y;
            cDilekceEkKayitResult result = new cDilekceEkKayitResult();
            cResponseModel<cDilekceEkKayitResult> oResponseModel = new cResponseModel<cDilekceEkKayitResult>();

            string dilekceNo;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = @" select trim(isnull(DilekceNo, '???'))
                                FROM [UGM_ERP].[dbo].[DilekceTakipMstr]
                                where 1=1
                                and Id = " + param.id;


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        bool flag = reader.Read();
                        dilekceNo = reader.GetString(0);
                        if (!flag || dilekceNo == "???")
                        {
                            result.ResultStatus = false;
                            result.ResultMessage = new List<string>() { "İlgili kaydın Dilekçe numarası bulunamadı." };
                            oResponseModel.errorModel.ErrorCode = -1;
                            oResponseModel.errorModel.ErrorMessage = "İlgili kaydın Dilekçe numarası bulunamadı." ;
                            return oResponseModel;
                        }
                    }
                }


                foreach (string? base64String in param.resimListesi)
                {
                    byte[] binaryData = Convert.FromBase64String(base64String);

                    // Hedef dosya yolu
                    string yeniDosyaYolu = cCommon.DosyaYoluDilekce;
#if DEBUG
                    yeniDosyaYolu = @"C:\\resim_" + param.id + "_" + Guid.NewGuid().ToString() + ".jpg";
#endif

                    // Dosyaya yaz
                    FilingHelper.FileSave(binaryData, yeniDosyaYolu, "resim_" + param.id + "_" + Guid.NewGuid().ToString() + ".jpg");
                    //convert(datetime, '20181025', 112)

                    sql = @"insert into [UGM_ERP].[dbo].[DilekceTakipEkler] 
		                            ( [Tarih]
		                            , [Dokumantipi]
		                            , [KulKodEkleyen]
		                            , [Dosya]
		                            , [DilekceNo]
		                            , [KayitTarihi]
		                            , [KayitKulKod]
		                            , [MstrId]
		                            )
                              values (convert(datetime, '" + param.tarih + @"', 112)
		                            , '" + param.kayitNo + @"'
		                            , '" + param.kullaniciKodu + @"'
		                            , '" + yeniDosyaYolu + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyyMMdd") + @"'
		                            , '" + dilekceNo + @"'
		                            , getdate()
		                            , '" + param.kullaniciKodu + @"'
		                            , " + param.id + @"
		                            )";


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        int insertedRow = command.ExecuteNonQuery();

                        if (insertedRow == 0)
                        {
                            result.ResultStatus = false;
                            result.ResultMessage = new List<string>() { "[UGM_ERP].[dbo].[DilekceTakipEkler] tablosuna kayıt eklenemedi." };
                            oResponseModel.errorModel.ErrorCode = -2;
                            oResponseModel.errorModel.ErrorMessage = "[UGM_ERP].[dbo].[DilekceTakipEkler] tablosuna kayıt eklenemedi.";
                            return oResponseModel;
                        }
                    }
                }
                result.ResultStatus = true;
                connection.Close();
            }

            return oResponseModel;
        }
    }
}
