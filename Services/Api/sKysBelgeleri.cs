using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.Api;

namespace MusteriMobilUygulamaAPI.Services.Api
{
    public class sKysBelgeleri
    {
        public static cResponseModel<cKysBelgeleri>? getKysBelgeleri()
        {
            cResponseModel<cKysBelgeleri> oResponseModel = new cResponseModel<cKysBelgeleri>();
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = cCommon.ConnectionString;

                List<cKysBelgeleri> oKysBelgeleriS = new List<cKysBelgeleri>();

                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                connection.Open();

                string sql = @" SELECT [BelgeAdi],[BelgeUrl] FROM [UGM_ERP].[dbo].[BelgeSureTakipTumDosyalar] ";


                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader readerForLikes = command.ExecuteReader();
                DataTable dataTableForLikes = new DataTable();
                dataTableForLikes.Load(readerForLikes);
                connection.Close();
                             
                foreach (DataRow item in dataTableForLikes.Rows)
                {
                        cKysBelgeleri oKysBelgeleri = new cKysBelgeleri();
                        oKysBelgeleri.BelgeAdi = item["BelgeAdi"] == DBNull.Value ? "" : item["BelgeAdi"].ToString();
                        oKysBelgeleri.BelgeUrl = item["BelgeUrl"] == DBNull.Value ? "" : item["BelgeUrl"].ToString();
                        oKysBelgeleriS.Add(oKysBelgeleri);
                }
                oResponseModel.Data = oKysBelgeleriS;                            
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
