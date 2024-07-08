using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.Api;

namespace MusteriMobilUygulamaAPI.Services.Api
{
    public class sBekleyenIsler
    {
        public static cResponseModel<cBekleyenIsler>? getBekleyenIsler(string firma_no, string IslemTipi, string Tip, string kullanici, string parola, string udid)
        {
            cResponseModel<cBekleyenIsler> oResponseModel = new cResponseModel<cBekleyenIsler>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            List<cBekleyenIsler> items = new List<cBekleyenIsler>();

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            string sql = $@" exec Evrim_DB.dbo.emobil_BekleyenIsler {firma_no}, '{IslemTipi}', '{Tip}', '{kullanici}', '{parola}', '{udid}' ";

            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader readerForLikes = command.ExecuteReader();
            DataTable dataTableForLikes = new DataTable();
            dataTableForLikes.Load(readerForLikes);
            connection.Close();
                    
            if (dataTableForLikes?.Rows.Count > 0 && dataTableForLikes?.Rows[0]?.ItemArray[0]?.ToString() == "GİRİŞ YETKİNİZ BULUNMAMAKTA")
            {
                oResponseModel.errorModel.ErrorCode = -1;
                oResponseModel.errorModel.ErrorMessage = "GİRİŞ YETKİNİZ BULUNMAMAKTA";
                return oResponseModel;
            }
            dataTableForLikes = dataTableForLikes == null ? new DataTable() : dataTableForLikes;

            foreach (DataRow row in dataTableForLikes.Rows)
            {
                    cBekleyenIsler item = new cBekleyenIsler();
                    
                    item.Ay = row["Ay"] == DBNull.Value ? "" : row["Ay"].ToString();
                    item.Dosya_no = row["Dosya_no"] == DBNull.Value ? "" : row["Dosya_no"].ToString();
                    item.BeyannameNumarasi = row["BeyannameNumarasi"] == DBNull.Value ? "" : row["BeyannameNumarasi"].ToString();
                    item.Referans = row["Referans"] == DBNull.Value ? "" : row["Referans"].ToString();
                    item.Gonderen = row["Gonderen"] == DBNull.Value ? "" : row["Gonderen"].ToString();
                    item.FaturaNo = row["FaturaNo"] == DBNull.Value ? "" : row["FaturaNo"].ToString();
                    item.FaturaBedeli = row["FaturaBedeli"] == DBNull.Value ? "" : row["FaturaBedeli"].ToString();
                    item.Doviz = row["Doviz"] == DBNull.Value ? "" : row["Doviz"].ToString();
                    item.Miktar = row["Miktar"] == DBNull.Value ? "" : row["Miktar"].ToString();
                    item.Kap = row["Kap"] == DBNull.Value ? "" : row["Kap"].ToString();
                    item.BrutKG = row["BrutKG"] == DBNull.Value ? "" : row["BrutKG"].ToString();
                    item.Gumruk = row["Gumruk"] == DBNull.Value ? "" : row["Gumruk"].ToString();
                    item.Nakliyeci = row["Nakliyeci"] == DBNull.Value ? "" : row["Nakliyeci"].ToString();
                    item.CikisVasitasi = row["CikisVasitasi"] == DBNull.Value ? "" : row["CikisVasitasi"].ToString();
                    item.BulunduguYer = row["BulunduguYer"] == DBNull.Value ? "" : row["BulunduguYer"].ToString();
                    item.OzetBeyanNo = row["OzetBeyanNo"] == DBNull.Value ? "" : row["OzetBeyanNo"].ToString();
                    item.OzetBeyanTar = row["OzetBeyanTar"] == DBNull.Value ? "" : row["OzetBeyanTar"].ToString();
                    item.HouseNo = row["HouseNo"] == DBNull.Value ? "" : row["HouseNo"].ToString();
                    item.Durumu = row["Durumu"] == DBNull.Value ? "" : row["Durumu"].ToString();
                    item.DurumuTarihSaat = row["DurumuTarihSaat"] == DBNull.Value ? "" : row["DurumuTarihSaat"].ToString();

                    items.Add(item);
            }
            oResponseModel.Data = items;                            
            return oResponseModel;
        }
    }
}
