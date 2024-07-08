using System;
using System.Data.SqlClient;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.IsTakip;

namespace MusteriMobilUygulamaAPI.Services.IsTakip
{
    public static class sTarimOdemeKayitGorseliYukle
    {
        public static cResponseModel<int> post(cTarimOdemeKayitGorseliYukle param)
        {
            //int y = 0, x = 1 / y;
            string ret = "";

            cResponseModel<int> oResponseModel = new cResponseModel<int>();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;
            SqlTransaction? myTran = null;
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                try
                {
                    connection.Open();
                    myTran = connection.BeginTransaction();

                    string logs = @"dosya_no = " + param.dosya_no + " kullanici_kodu = " + param.kullanici_kodu
                                + " urun_bildirim_no = " + param.urun_bildirim_no + " kayit_no = " + param.kayit_no
                                + " odeme_tipi = " + param.odeme_tipi + " odeme_tipi_id = " + param.odeme_tipi_id;


                    string sql = @" INSERT INTO UGM_ERP.dbo.Tarim_Gida_OdemeTakip_Logs (UrunBildirimNo, Logs)
                                    VALUES('" + param.urun_bildirim_no + "', '" + logs + "') ";

                    try
                    {
                        using (SqlCommand command = new SqlCommand(sql, connection, myTran))
                        {
                            int insertedRow = command.ExecuteNonQuery();

                            if (insertedRow == 0)
                            {
                                ret = "{'error':'UGM_ERP.dbo.Tarim_Gida_OdemeTakip_Logs tablosuna kayıt yapılamadı.'}";
                                connection.Close();
                                myTran = null;
                                oResponseModel.errorModel.ErrorCode = -1;
                                oResponseModel.errorModel.ErrorMessage = "UGM_ERP.dbo.Tarim_Gida_OdemeTakip_Logs tablosuna kayıt yapılamadı.";
                                return oResponseModel;
                            }
                        }
                    }
                    catch (Exception pExc)
                    {
                        ret = "{'error':'UGM_ERP.dbo.Tarim_Gida_OdemeTakip_Logs tablosuna kayıt yapılamadı. Exception : " + pExc.Message + "'}";
                        connection.Close();
                        myTran = null;
                        oResponseModel.errorModel.ErrorCode = -2;
                        oResponseModel.errorModel.ErrorMessage = "UGM_ERP.dbo.Tarim_Gida_OdemeTakip_Logs tablosuna kayıt yapılamadı. Exception : " + pExc.Message;
                        return oResponseModel;
                    }

                    string kayit_path = "";
                    try
                    {
                        kayit_path = FilingHelper.SaveTarimOdemeFiles(param.images, param.urun_bildirim_no);
                    }
                    catch (Exception pExc)
                    {
                        ret = "Resim dosyaları kaydedilemedi. " + pExc.Message;
                        connection.Close();
                        myTran = null;
                        oResponseModel.errorModel.ErrorCode = -3;
                        oResponseModel.errorModel.ErrorMessage = "Resim dosyaları kaydedilemedi. Exception : " + pExc.Message + " : " + pExc.StackTrace;
                        return oResponseModel;
                    }

                    sql = $@"MERGE UGM_ERP.dbo.Tarim_Gida_OdemeTakip AS t 
                            USING (SELECT '{param.urun_bildirim_no}' UrunBildirimNo) AS s
                            ON t.UrunBildirimNo = s.UrunBildirimNo 
                            -- For Inserts
                            WHEN NOT MATCHED THEN
                                INSERT (DosyaNo, KayitNo, KayitPath, OdemeTipi)
                                VALUES ('{param.dosya_no}', '{param.kayit_no}', '{kayit_path}', '{param.odeme_tipi}')
                            -- For Updates
                            WHEN MATCHED THEN UPDATE SET  t.KayitNo   = '{param.kayit_no}' 
                                                        , t.KayitPath = '{kayit_path}'
                                                        , t.OdemeTipi = '{param.odeme_tipi}'
                                                        , t.DosyaNo   = '{param.dosya_no}'; ";
                    if (param.odeme_tipi == "1")
                        sql = @"INSERT INTO UGM_ERP.dbo.Tarim_Gida_OdemeTakip (DosyaNo, KayitNo, KayitPath, OdemeTipi)
                                VALUES('" + param.dosya_no + "', '" + param.kayit_no + "', '" + kayit_path + "', '" + param.odeme_tipi + "')";

                    try
                    {
                        using (SqlCommand command = new SqlCommand(sql, connection, myTran))
                        {
                            int affectedRow = command.ExecuteNonQuery();

                            if (affectedRow == 0)
                            {
                                if (param.odeme_tipi == "1")
                                    ret = "{'success': 'Tarım gıda ödeme takip bilgileri eklenEMEdi.'}";
                                else if (param.odeme_tipi == "2")
                                    ret = "{'success': 'Tarım gıda ödeme takip bilgileri güncellenEMEdi.'}";
                                else ret = "{'error': 'odeme_tipi 1 ya da 2 olmalıdır!!!'}";
                                connection.Close();
                                myTran = null;
                                oResponseModel.errorModel.ErrorCode = -4;
                                oResponseModel.errorModel.ErrorMessage = "odeme_tipi 1 ya da 2 olmalıdır!!!";
                                return oResponseModel;
                            }
                        }
                    }
                    catch (Exception pExc)
                    {
                        ret = "UGM_ERP.dbo.Tarim_Gida_OdemeTakip tablosuna erişilemedi. Exception : " + pExc.Message;
                        connection.Close();
                        myTran = null;
                        oResponseModel.errorModel.ErrorCode = -5;
                        oResponseModel.errorModel.ErrorMessage = "UGM_ERP.dbo.Tarim_Gida_OdemeTakip tablosuna erişilemedi. Exception : " + pExc.Message;
                        return oResponseModel;
                    }

                    try
                    {
                        sql = @"EXEC Evrim_DB.dbo.ark_KuryeAddRecordToIsTakipT '" + param.dosya_no + "', '290', 'T', '" + param.kullanici_kodu + "'";

                        using (SqlCommand command = new SqlCommand(sql, connection, myTran))
                        {
                            SqlDataReader reader = command.ExecuteReader();
                            reader.Close();                            
                        }
                    }
                    catch (Exception pExc)
                    {
                        ret = "EXEC Evrim_DB.dbo.ark_KuryeAddRecordToIsTakipT prosedürüne erişilemedi. Exception : " + pExc.Message;
                        connection.Close();
                        myTran = null;
                        oResponseModel.errorModel.ErrorCode = -6;
                        oResponseModel.errorModel.ErrorMessage = "EXEC Evrim_DB.dbo.ark_KuryeAddRecordToIsTakipT prosedürüne erişilemedi. Exception : " + pExc.Message;
                        return oResponseModel;
                    }
                    //connection.Close();
                    myTran.Commit();
                    ret = "{'success': 'Tarım gıda ödeme takip bilgileri güncellendi.'}";
                    oResponseModel.errorModel.ErrorMessage = "Tarım gıda ödeme takip bilgileri güncellendi.";
                }

                catch (Exception pExc)
                {          
                    connection.Close();
                    myTran?.Rollback();
                    oResponseModel.errorModel.ErrorCode = -6;
                    oResponseModel.errorModel.ErrorMessage = "Exception : " + pExc.Message;
                    return oResponseModel;
                }
                finally
                {
                    myTran = null;
                    
                }
            }
            return oResponseModel;
        }
    }
}
