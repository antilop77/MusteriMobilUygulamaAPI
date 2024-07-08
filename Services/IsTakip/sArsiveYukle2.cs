using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.IsTakip;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MusteriMobilUygulamaAPI.Services.IsTakip
{
    public static class sArsiveYukle2
    {
        public static cResponseModel<int> post(cArsiveYukle2 param)
        {
            //int y=0, x = 1/y;
            cResponseModel<int> oResponseModel = new cResponseModel<int>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            string[] fileNoList = param?.Dosyano?.Replace(" ", "").Split(",");
            /*
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {                    
                connection.Open();       
                    
                string importFileNos = param?.Dosyano;
                string exportFileNos = param?.Dosyano;

                string sql = @" SELECT Gonderen_No
                                FROM Evrim_DB.dbo.GGBmastr
                                WHERE 1=1
                                and Dosya_No in ({0}) 
                                union all 
                                SELECT GonderenNo as Gonderen_No
                                FROM Evrim_DB.dbo.GCBmstr
                                WHERE 1=1
                                and Dosya_No in ({0})";

                int firmaCnt = 0;
                
                SqlCommand sqlCommand = new SqlCommand(sql, connection);

                var idParameterList = new List<string>();
                var index = 0;                

                // Create a SqlParameter for each element in the array called "@idParam0", "@idParam1"... and add to list idParameterList
                foreach (var fileNo in fileNoList)
                {
                    var paramName = "@idParam" + index;
                    sqlCommand.Parameters.AddWithValue(paramName, fileNo);
                    idParameterList.Add(paramName);
                    index++;
                }

                // Finalise SQL String for datainput - DONE AND WORKS
                sqlCommand.CommandText = String.Format(sql, string.Join(",", idParameterList));

                                
                //using (SqlCommand command = new SqlCommand(sql, connection))
                {                    
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {                 
                        while (reader.Read())
                        {
                            firmaCnt++;
                            if (firmaCnt > 1)
                                break;
                        }                            
                    }
                }
                connection.Close();

                if (firmaCnt > 1)
                {
                    oResponseModel.errorModel.ErrorCode = -1;
                    oResponseModel.errorModel.ErrorMessage = "Tüm dosyalar tek bir firmaya ait olmalıdır. Gönderilen dosya listesinde tespit edilen firma sayısı: " + firmaCnt.ToString();  
                    return oResponseModel;
                }
            }
            */

            string isTakipKodu = "";
            string arsiv_dokuman_adi = "";
            string istakipaciklama = "";

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = @" SELECT 
                                    ID,
                                    MOBIL_GORUNUM_ISMI,
                                    IS_TAKIP_KODU,
                                    IS_TAKIP_ACIKLAMASI,
                                    ARSIV_DOSYA_SINIFI,
                                    ARSIV_DOKUMAN_ADI
                                FROM UGM_ERP.dbo.MOBIL_FOTOGRAF_TIP
                                WHERE ID = " + param?.Fototip;

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            isTakipKodu = reader.GetString(2);
                            arsiv_dokuman_adi = reader.GetString(5);
                            istakipaciklama = reader.GetString(3);
                            break;
                        }
                    }
                }
                connection.Close();
            }


            int? adet = param?.Images?.Count();
            if (isTakipKodu == "290") //TARIM_BILDIRIM_KODU = "290"
                adet = 1;

            string? tip = param?.Tip;
            if (tip?.ToUpper() == "K")
                tip = "U";

            string arsivFolderPath = "";

            for (int i = 0; i < fileNoList?.Length; i++)
            {
                string todayStr = DateTime.Now.ToString("yyyyMMdd");
                string fileGuid = Guid.NewGuid().ToString();
                for (int j = 0; j < param?.Images?.Count; j++)
                {
                    byte[] image = Convert.FromBase64String(param?.Images?[j].Base64);
                    arsivFolderPath = FilingHelper.FileSave(image, cCommon.ArsivPath + todayStr + Path.DirectorySeparatorChar + fileGuid, param?.Images?[j].Filename);
                }

                cArsiveYukle2Data obj = new cArsiveYukle2Data();
                obj.Path = arsivFolderPath;
                obj.ArsivTip = "Resim";
                obj.ArsivDocName = arsiv_dokuman_adi;
                IsTakip2 istakip = new IsTakip2();
                List<IsTakip2> istakips = new List<IsTakip2>();
                istakips.Add(istakip);

                istakip.IsTakipKod = isTakipKodu;
                istakip.IsTakipAciklama = istakipaciklama;
                istakip.Tarih = DateTime.Now.ToString();
                istakip.Adet = adet.ToString();
                obj.IsTakip = istakips;
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

                string sql = @"INSERT INTO UGM_ERP.dbo.ArsivAktarim (Tip, DosyaNo, Params, Completed, KuyrukNo)
                              values ('" + tip?.ToString() + "', '" + fileNoList[i].ToString() + "', '" + json + "', 0, 3)";
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        int insertedRow = command.ExecuteNonQuery();

                        if (insertedRow == 0)
                        {
                            //result.ResultStatus = false;
                            //result.ResultMessage = new List<string>() { "[UGM_ERP].[dbo].[DilekceTakipEkler] tablosuna kayıt eklenemedi." };
                            //return result;
                        }
                        connection.Close();
                    }
                }
            }

            oResponseModel = new cResponseModel<int>();
            oResponseModel.errorModel.ErrorMessage = "Arsiv icin kayitlar eklendi.";
            return oResponseModel;
        }
    }
}
