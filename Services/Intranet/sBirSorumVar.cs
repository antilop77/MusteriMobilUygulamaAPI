using MusteriMobilUygulamaAPI.Models.Api;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Services.Api;
using System.Data.SqlClient;
using System.Data;
using MusteriMobilUygulamaAPI.Models.Intranet;
using System.Numerics;
using System.Globalization;
using MusteriMobilUygulamaAPI;
using System.Collections;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace MusteriMobilUygulamaAPI.Services.Intranet
{
    public class sBirSorumVar
    {
        public static cResponseModel<int> postBirSorumVar(string konu, string soru, string username)
        {            
            List<int> items = new List<int>();
            cResponseModel<int> oResponseModel = new cResponseModel<int>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionStringForKurumsal;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                
                string sql = $@" INSERT INTO Intranet.dbo.BirSorumVar (Konu,Soru,UserName) VALUES ( '{konu}', '{soru}', '{username}') ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    int cntInserted = command.ExecuteNonQuery();
                    if (cntInserted > 0)
                    {
                        items.Add(1);
                        oResponseModel.errorModel.ErrorMessage = "Soru kaydı başarılı bir şekilde eklendi.";
                    }
                    else
                    {
                        oResponseModel.errorModel.ErrorCode = -1;
                        oResponseModel.errorModel.ErrorMessage = "Kayıt edilirken hata alındı ";
                    }
                }
                connection.Close();
            }
            
            oResponseModel.Data = items;
            return oResponseModel;
        }
    }
}
