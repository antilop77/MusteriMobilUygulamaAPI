using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.IsTakip;
using System;
using System.Data.SqlClient;
using WireMock.Admin.Mappings;
using WireMock.Admin.Requests;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MusteriMobilUygulamaAPI.Services.IsTakip
{
    public static class sIsTakipNew2
    {
        public static cResponseModel<int> IsTakipNewIki(cIsTakipNew2 param)
        {
            //int y=0, x = 1/y;
            cResponseModel<int> oResponseModel = new cResponseModel<int>();
            try
            {
                /* param?.resim her zaman bos geliyor :-(
                string todayStr = DateTime.Now.ToString("yyyyMMdd");
                string fileGuid = Guid.NewGuid().ToString();
                byte[] image = Convert.FromBase64String(param?.resim);
                string arsivFolderPath = FilingHelper.FileSave(image, cCommon.ArsivPath + todayStr + Path.DirectorySeparatorChar + fileGuid, "resim.jpg");

                cIsTakipNew2Data oIsTakipNew2Data = new cIsTakipNew2Data();
                oIsTakipNew2Data.path = arsivFolderPath;
                oIsTakipNew2Data.arsivtip = "Resim";
                oIsTakipNew2Data.istakip[0].istakipkod = param.is_takip_kodu;
                oIsTakipNew2Data.istakip[0].tarih = DateTime.Now.ToString();
                oIsTakipNew2Data.istakip[0].adet = 0;

                oResponseModel.errorModel.ErrorCode = 0;
                oResponseModel.errorModel.ErrorMessage = "Kayit Eklendi";
                oResponseModel.Data?.Add(1);
                */
                string sql = "";
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = cCommon.ConnectionString;
    
                sql = @" declare @myvar datetime select @myvar = getdate() exec [dbo].[eistakipmobil_istakipnew2] '" + param.kod + "', '" + param.imei + "', '" + param.okuma_tipi + "', '" + param.is_takip_kodu + @"', '"
                                                                + param.aciklama + "', '" + param.dosya_no + "', '" + param.resim + "', '" + param.resim_aciklama + @"', @myvar, '"
                                                                + param.gecikme_sebebi + "', " + param.adet;
                                        
                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                
                connection.Open();

                SqlCommand command = new SqlCommand(sql, connection);
                
                command.ExecuteNonQuery();
                        
                connection.Close();
                
                oResponseModel.errorModel.ErrorCode = 0;
                oResponseModel.errorModel.ErrorMessage = "1 Kayit Eklendi"; 
                oResponseModel.Data?.Add(1);
                return oResponseModel;
            }
            catch (Exception ex)
            {
                oResponseModel.errorModel.ErrorCode = -1;
                oResponseModel.errorModel.ErrorMessage = ex.Message;
                return oResponseModel;
            }
            
        }
    }
}
