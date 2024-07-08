using System.Collections.Generic;
using System.Data.SqlClient;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.IsTakip;

namespace MusteriMobilUygulamaAPI.Services.IsTakip
{
    public static class sDosyaBilgi
    {
        public static cResponseModel<cDosyaBilgi> get(string kod, string imei, string belge_no)
        {
            //int y=0, x = 1/y;
            cDosyaBilgi oDosyaBilgi = new cDosyaBilgi();
            cResponseModel<cDosyaBilgi> oResponseModel = new cResponseModel<cDosyaBilgi>();
            Beyanname beyanname = new Beyanname();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            SqlConnection connection = new SqlConnection(builder.ConnectionString);

            connection.Open();

            string sql = @" exec [Evrim_DB].[dbo].[eistakipmobil_beyannamebilgi] '" + belge_no + "'";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        beyanname.beyanname_no = reader.GetString(0);
                        break;
                    }
                }
            }

            List<MusteriTemsilcisi> musteri_temsilcileri = new List<MusteriTemsilcisi>();

            sql = @" exec [Evrim_DB].[dbo].[eistakipmobil_dosyakullanicibilgi] '" + belge_no.Replace("H", "").Replace("T", "") + "'";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MusteriTemsilcisi item = new MusteriTemsilcisi();
                        item.personel = reader["Personel"] == DBNull.Value ? null : reader.GetString(0);
                        item.kodu = reader["Kodu"] == DBNull.Value ? null : reader.GetString(1);
                        item.mail = reader["mail"] == DBNull.Value ? null : reader.GetString(2);
                        item.cep_no1 = reader["cep_no1"] == DBNull.Value ? null : reader.GetString(3);
                        item.dahili_no = reader["dahili_no"] == DBNull.Value ? null : reader.GetString(4);
                        item.kisa_kod = reader["kisa_kod"] == DBNull.Value ? null : reader.GetString(5);

                        musteri_temsilcileri.Add(item);
                    }
                }
            }

            List<Data> data = new List<Data>();

            sql = @" exec [Evrim_DB].[dbo].[eistakipmobil_dosyabilgi] '" + kod + "', '" + imei + "', '" + belge_no + "'";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Data item = new Data();
                        item.tip = reader["tip"] == DBNull.Value ? null : reader.GetString(0);
                        item.aciklama = reader["aciklama"] == DBNull.Value ? null : reader.GetString(1);
                        item.bilgi = reader["bilgi"] == DBNull.Value ? null : reader.GetString(2);

                        data.Add(item);
                    }
                }
            }
            connection.Close();

            oDosyaBilgi.beyanname = beyanname;
            oDosyaBilgi.musteri_temsilcisi = musteri_temsilcileri;
            oDosyaBilgi.data = data;

            List<cDosyaBilgi> list = new List<cDosyaBilgi>();
            list.Add(oDosyaBilgi);
            oResponseModel.Data = list;
            return oResponseModel;
        }
    }
}
