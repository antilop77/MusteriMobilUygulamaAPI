using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.Api;

namespace MusteriMobilUygulamaAPI.Services.Api
{
    public class sIletisim
    {
        public static cResponseModel<cIletisim>? getEmobilOfis()
        {
            cResponseModel<cIletisim> oResponseModel = new cResponseModel<cIletisim>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            List<cIletisim> items = new List<cIletisim>();

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            string sql = $@" SELECT  ID, OfisAdi, se.Sehir,
                                Case 
                                when e.ID in ('45','46','70','75','80','84','95','101','105') then 'ANKARA BÖLGE'
                                when e.ID in ('51','81','82','83','103') then 'BATI MARMARA BÖLGE MÜDÜRLÜĞÜ'
                                when e.ID in ('55','56','57','58','60','62','64','87','93','94','99') then 'BÖLGE KOORDİNÖRLÜĞÜ '
                                when e.ID in ('43','44','53','54','78','110') then 'ÇUKUROVA BÖLGE'
                                when e.ID in ('47','50','67','68','69','77','96','98','102','106','107','108') then 'EGE BÖLGE'
                                when e.ID in ('48','49','52','76','85','97') then 'GÜNEY MARMARA'
                                when e.ID in ('59','63','65','109') then 'İSTANBUL ANADOLU YAKASI BÖLGE '
                                when e.ID in ('71','73','74','88') then 'KOCAELİ BÖLGE'
		                else 'DİĞER' end as Bolgesi,
                                Adres as Ilce, 
                                Telefon1, 
                                Telefon2, 
                                Fax1, 
                                Fax2, 
                                Yetkili, 
                                Koordinat , 
                                ''  as Bolgesi_en
                        FROM Evrim_DB.dbo.emobilOfis e, Evrim_DB.dbo.Sehir se
                        Where 1=1
		                and e.Sehir=se.Sehir_Kodu
                        ORDER BY  SiraNo ";

            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader readerForLikes = command.ExecuteReader();
            DataTable dataTableForLikes = new DataTable();
            dataTableForLikes.Load(readerForLikes);
            connection.Close();
                             
            foreach (DataRow row in dataTableForLikes.Rows)
            {
                    cIletisim item = new cIletisim();
                    item.id = row["Id"] == DBNull.Value ? 0 : Int32.Parse(row["Id"].ToString());
                    item.ofisadi = row["ofisadi"] == DBNull.Value ? "" : row["ofisadi"].ToString();
                    item.sehir = row["sehir"] == DBNull.Value ? "" : row["sehir"].ToString();
                    item.bolgesi = row["bolgesi"] == DBNull.Value ? "" : row["bolgesi"].ToString();
                    item.ilce = row["Ilce"] == DBNull.Value ? "" : row["Ilce"].ToString();
                    item.telefon1 = row["telefon1"] == DBNull.Value ? "" : row["telefon1"].ToString();
                    item.telefon2 = row["telefon2"] == DBNull.Value ? "" : row["telefon2"].ToString();
                    item.fax1 = row["fax1"] == DBNull.Value ? "" : row["fax1"].ToString();
                    item.fax2 = row["fax2"] == DBNull.Value ? "" : row["fax2"].ToString();
                    item.yetkili = row["yetkili"] == DBNull.Value ? "" : row["yetkili"].ToString();
                    item.koordinat = row["koordinat"] == DBNull.Value ? "" : row["koordinat"].ToString();
                    item.bolgesi_en = row["bolgesi_en"] == DBNull.Value ? "" : row["bolgesi_en"].ToString();
                    items.Add(item);
            }
            oResponseModel.Data = items;                            
            return oResponseModel;
        }

        //public static cResponseModel<int>? postSendMail(cSendMail oSendMail)
        //{
        //    cResponseModel<int> oResponseModel = new cResponseModel<int>();

        //    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        //    builder.ConnectionString = cCommon.ConnectionString;

        //    SqlConnection connection = new SqlConnection(builder.ConnectionString);
        //    connection.Open();
                
        //    string sql = $@" insert into UGM_ERP.dbo.UgmSendMail(mSubject,mFrom,mPassword,mTo,mCc,mBcc,mBody,CreateTime) values
        //                 ('{oSendMail.subject}', 'bildirim@ugm.com.tr', 'Bldr.2021', '{oSendMail.to}',' {oSendMail.cc}', '{oSendMail.bcc}', '{oSendMail.body}', getdate()) ";

        //    using (SqlCommand command = new SqlCommand(sql, connection))
        //    {
        //        int x = command.ExecuteNonQuery();
        //        if (x > 0)
        //        {
        //            oResponseModel.errorModel.ErrorMessage = "islem tamam";
        //        }
        //        else
        //        {
        //            oResponseModel.errorModel.ErrorCode = -1;
        //            oResponseModel.errorModel.ErrorMessage = "islem basarisiz";
        //        }
        //    }
        //    connection.Close();
                                      
        //    return oResponseModel;
        //}
    }
}
