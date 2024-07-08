using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.Api;

namespace MusteriMobilUygulamaAPI.Services.Api
{
    public class sEmobilBlogLikes
    {
        public static cResponseModel<cEmobilBlogLike>? getEmobilBlogLikes(int pBlogId, bool withPhoto)
        {
            cResponseModel<cEmobilBlogLike> oResponseModel = new cResponseModel<cEmobilBlogLike>();
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = cCommon.ConnectionString;

                List<cEmobilBlogLike> oEmobilBlogLikes = new List<cEmobilBlogLike>();

                    SqlConnection connection = new SqlConnection(builder.ConnectionString);
                    connection.Open();

                    string sql = @" select blogId,
                                        Id,
	                                    externalId,
                                        internalId,
                                        type,
                                        date
                                    from Evrim_DB.dbo.emobilLike with(nolock)
                                    where 1=1 
                                    and blogId = " + pBlogId.ToString();


                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader readerForLikes = command.ExecuteReader();
                    DataTable dataTableForLikes = new DataTable();
                    dataTableForLikes.Load(readerForLikes);
                    connection.Close();
                             
            
                foreach (DataRow item in dataTableForLikes.Rows)
                {
                    if ((int)item["blogId"] == pBlogId)
                    {
                        cEmobilBlogLike oEmobilLike = new cEmobilBlogLike();
                        oEmobilLike.id = item["Id"] == DBNull.Value ? -1 : (int)item["Id"];
                        oEmobilLike.type = item["type"] == DBNull.Value ? -1 : (int)item["type"];
                        oEmobilLike.date = item["date"] == DBNull.Value ? null : (DateTime)item["date"];
                        oEmobilLike.user = cCommon.getUserInfoForPost(item["internalId"] == DBNull.Value ? -1 : (int)item["internalId"]
                                                                    , item["externalId"] == DBNull.Value ? -1 : (int)item["externalId"], withPhoto);
                        oEmobilBlogLikes.Add(oEmobilLike);
                    }
                }
                oResponseModel.Data = oEmobilBlogLikes;                            
                return oResponseModel;
            } catch (Exception ex)
            {
                oResponseModel.errorModel.ErrorCode = -1;
                oResponseModel.errorModel.ErrorMessage = ex.Message;
                return oResponseModel;
            }
        }
    }
}
