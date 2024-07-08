using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.Api;

namespace MusteriMobilUygulamaAPI.Services.Api
{
    public class sBilgilendirme
    {
        public static cResponseModel<cDil>? getDil()
        {
            cResponseModel<cDil> oResponseModel = new cResponseModel<cDil>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            List<cDil> items = new List<cDil>();

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            string sql = $@" exec Evrim_db.dbo.emobil_Dil ";


            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader readerForLikes = command.ExecuteReader();
            DataTable dataTableForLikes = new DataTable();
            dataTableForLikes.Load(readerForLikes);
            connection.Close();
                             
            foreach (DataRow row in dataTableForLikes.Rows)
            {
                    cDil item = new cDil();
                    item.no = row["no"] == DBNull.Value ? 0 : Int32.Parse(row["no"].ToString());
                    item.dil = row["dil"] == DBNull.Value ? "" : row["dil"].ToString();
                    items.Add(item);
            }
            oResponseModel.Data = items;                            
            return oResponseModel;
        }

        public static cResponseModel<cEmobilSoruCevap>? getEmobilSoruCevap(string Tip, string Dil)
        {
            cResponseModel<cEmobilSoruCevap> oResponseModel = new cResponseModel<cEmobilSoruCevap>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            List<cEmobilSoruCevap> items = new List<cEmobilSoruCevap>();

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            string sql = $@" SELECT Soru, Cevap
	                        FROM Evrim_db.dbo.emobilSoruCevap
	                        WHERE 1=1
	                        and Tip = '{Tip}' 
	                        and Dil = '{Dil}'
                            and Pasif = 'Aktif'
	                        Order by Soru ";


            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader readerForLikes = command.ExecuteReader();
            DataTable dataTableForLikes = new DataTable();
            dataTableForLikes.Load(readerForLikes);
            connection.Close();
                             
            foreach (DataRow row in dataTableForLikes.Rows)
            {
                    cEmobilSoruCevap item = new cEmobilSoruCevap();
                    item.soru = row["soru"] == DBNull.Value ? "" : row["soru"].ToString();
                    item.cevap = row["cevap"] == DBNull.Value ? "" : row["cevap"].ToString();
                    items.Add(item);
            }
            oResponseModel.Data = items;                            
            return oResponseModel;
        }

        public static cResponseModel<cEmobilSoruCevapTip>? getEmobilSoruCevapTip(string Dil)
        {
            cResponseModel<cEmobilSoruCevapTip> oResponseModel = new cResponseModel<cEmobilSoruCevapTip>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            List<cEmobilSoruCevapTip> items = new List<cEmobilSoruCevapTip>();

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            string sql = $@" SELECT Tip,Aciklama,Dil
	                        FROM emobilSoruCevapTip
	                        WHERE 1=1
                            and Dil = '{Dil}'
                            and aktif = 1
	                        ORDER BY Sira ";


            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader readerForLikes = command.ExecuteReader();
            DataTable dataTableForLikes = new DataTable();
            dataTableForLikes.Load(readerForLikes);
            connection.Close();
                             
            foreach (DataRow row in dataTableForLikes.Rows)
            {
                    cEmobilSoruCevapTip item = new cEmobilSoruCevapTip();
                    item.tip = row["tip"] == DBNull.Value ? "" : row["tip"].ToString();
                    item.aciklama = row["aciklama"] == DBNull.Value ? "" : row["aciklama"].ToString();
                    item.dil = row["dil"] == DBNull.Value ? "" : row["dil"].ToString();
                    items.Add(item);
            }
            oResponseModel.Data = items;                            
            return oResponseModel;
        }
    }
}
