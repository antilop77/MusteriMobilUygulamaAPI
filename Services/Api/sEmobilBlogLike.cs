using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.Api;

namespace MusteriMobilUygulamaAPI.Services.Api
{
    public static class sEmobilBlogLike
    {

        public static cResponseModel<int> postEmobilBlogLike(int blogId, int? pInternalId, int? pExternalId)
        {
            //int y=0, x = 1/y;    

            cResponseModel<int> oResponseModel = new cResponseModel<int>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = @" delete from [Evrim_DB].[dbo].[emobilLike] where 1=1 and blogId = " + blogId + " and (internalId = " + pInternalId + " or externalId = " +  pExternalId + ")";
		        
                SqlCommand command = new SqlCommand(sql, connection);
                
                int deletedRow = command.ExecuteNonQuery();
                List<int> ints = new List<int>();

                if (deletedRow == 0)
                {
                    sql = @" insert into [Evrim_DB].[dbo].[emobilLike] (blogId, externalId, internalId, type, date)
                                values(" + blogId + ", " + pExternalId + ", " + pInternalId + ", 1, getdate());";

                    command = new SqlCommand(sql, connection);
                    int insertedRow = command.ExecuteNonQuery();
                    ints.Add(insertedRow);                        
                    oResponseModel.errorModel.ErrorCode = 0;
                    oResponseModel.errorModel.ErrorMessage = "Beğenme (Like) eklendi.";
                }
                else
                {                        
                    ints.Add(deletedRow);                        
                    oResponseModel.errorModel.ErrorCode = 0;
                    oResponseModel.errorModel.ErrorMessage = "Beğenme (Like) silindi.";
                }
                oResponseModel.Data = ints;
                
                connection.Close();
            }            

            return oResponseModel;
        }
    }
}
