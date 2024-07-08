using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MmuAPI.Models;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.Api;

namespace MusteriMobilUygulamaAPI.Services.Api
{
    public class sEmobilIhracatIthalatRaporu
    {
        public static cResponseModel<cEmobilIthalatRaporu>? getEmobilIthalatRaporu(string firma_no, string tescil_tarihi_1, string tescil_tarihi_2, string kullanici, string parola, string udid)
        {
            cResponseModel<cEmobilIthalatRaporu> oResponseModel = new cResponseModel<cEmobilIthalatRaporu>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            List<cEmobilIthalatRaporu> items = new List<cEmobilIthalatRaporu>();

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            string sql = $@" exec Evrim_DB.dbo.emobil_IthalatRaporu {firma_no} , '{DateTime.Parse(tescil_tarihi_1)}' , '{DateTime.Parse(tescil_tarihi_2)}' ,'{kullanici}',  '{parola}' , '{udid}' ";

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
                    cEmobilIthalatRaporu item = new cEmobilIthalatRaporu();
                                                            
                    item.Dosya_No = row["Dosya_No"] == DBNull.Value ? "" : row["Dosya_No"].ToString();
                    item.Alici = row["Alici"] == DBNull.Value ? "" : row["Alici"].ToString();
                    item.Gonderen = row["Gonderen"] == DBNull.Value ? "" : row["Gonderen"].ToString();
                    item.Referans = row["Referans"] == DBNull.Value ? "" : row["Referans"].ToString();
                    item.Koli = row["Koli"] == DBNull.Value ? "" : row["Koli"].ToString();
                    item.Tir_Kntyner = row["Tir_Kntyner"] == DBNull.Value ? "" : row["Tir_Kntyner"].ToString();
                    item.Ulke = row["Ulke"] == DBNull.Value ? "" : row["Ulke"].ToString();
                    item.Gumruk = row["Gumruk"] == DBNull.Value ? "" : row["Gumruk"].ToString();
                    item.TeslimSekli = row["TeslimSekli"] == DBNull.Value ? "" : row["TeslimSekli"].ToString();
                    item.Beyan_Tarihi = row["Beyan_Tarihi"] == DBNull.Value ? "" : row["Beyan_Tarihi"].ToString();
                    item.Beyan_No = row["Beyan_No"] == DBNull.Value ? "" : row["Beyan_No"].ToString();
                    item.Fatura_Bedeli = row["Fatura_Bedeli"] == DBNull.Value ? "" : row["Fatura_Bedeli"].ToString();
                    item.Doviz = row["Doviz"] == DBNull.Value ? "" : row["Doviz"].ToString();
                    item.Doviz_Kuru = row["Doviz_Kuru"] == DBNull.Value ? "" : row["Doviz_Kuru"].ToString();
                    item.HouseNo = row["HouseNo"] == DBNull.Value ? "" : row["HouseNo"].ToString();
                    item.CIF_Tutar = row["CIF_Tutar"] == DBNull.Value ? "" : row["CIF_Tutar"].ToString();
                    item.MalBedeli = row["MalBedeli"] == DBNull.Value ? "" : row["MalBedeli"].ToString();
                    item.GumrukVergisi = row["GumrukVergisi"] == DBNull.Value ? "" : row["GumrukVergisi"].ToString();
                    item.Toplam_Vergi = row["Toplam_Vergi"] == DBNull.Value ? "" : row["Toplam_Vergi"].ToString();
                    item.Ist_Kiymet = row["Ist_Kiymet"] == DBNull.Value ? "" : row["Ist_Kiymet"].ToString();
                    item.Sigorta = row["Sigorta"] == DBNull.Value ? "" : row["Sigorta"].ToString();
                    item.Navlun = row["Navlun"] == DBNull.Value ? "" : row["Navlun"].ToString();
                    item.Navlun_Doviz = row["Navlun_Doviz"] == DBNull.Value ? "" : row["Navlun_Doviz"].ToString();
                    item.CIFDiger = row["CIFDiger"] == DBNull.Value ? "" : row["CIFDiger"].ToString();
                    item.CIFDiger_Doviz = row["CIFDiger_Doviz"] == DBNull.Value ? "" : row["CIFDiger_Doviz"].ToString();
                    item.YurtDisiDigerGider = row["YurtDisiDigerGider"] == DBNull.Value ? "" : row["YurtDisiDigerGider"].ToString();
                    item.Pul_Ord_KF = row["Pul_Ord_KF"] == DBNull.Value ? "" : row["Pul_Ord_KF"].ToString();
                    item.Ardiye = row["Ardiye"] == DBNull.Value ? "" : row["Ardiye"].ToString();
                    item.BankaKomisyonu = row["BankaKomisyonu"] == DBNull.Value ? "" : row["BankaKomisyonu"].ToString();
                    item.KKDF = row["KKDF"] == DBNull.Value ? "" : row["KKDF"].ToString();
                    item.YICI = row["YICI"] == DBNull.Value ? "" : row["YICI"].ToString();
                    item.KDVMatrahi = row["KDVMatrahi"] == DBNull.Value ? "" : row["KDVMatrahi"].ToString();
                    item.Nakliyeci = row["Nakliyeci"] == DBNull.Value ? "" : row["Nakliyeci"].ToString();
                    item.Net_Kilo = row["Net_Kilo"] == DBNull.Value ? "" : row["Net_Kilo"].ToString();
                    item.Brut_Kilo = row["Brut_Kilo"] == DBNull.Value ? "" : row["Brut_Kilo"].ToString();
                    item.Rejim = row["Rejim"] == DBNull.Value ? "" : row["Rejim"].ToString();
                    item.Kullanici = row["Kullanici"] == DBNull.Value ? "" : row["Kullanici"].ToString();
                    item.Varis_Gumruk = row["Varis_Gumruk"] == DBNull.Value ? "" : row["Varis_Gumruk"].ToString();
                    item.SinirdakiTS = row["SinirdakiTS"] == DBNull.Value ? "" : row["SinirdakiTS"].ToString();
                    item.DahiliTS = row["DahiliTS"] == DBNull.Value ? "" : row["DahiliTS"].ToString();
                    item.CikisVasitasi = row["CikisVasitasi"] == DBNull.Value ? "" : row["CikisVasitasi"].ToString();
                    item.FaturaNo = row["FaturaNo"] == DBNull.Value ? "" : row["FaturaNo"].ToString();
                    item.Fatura_Tarihi = row["Fatura_Tarihi"] == DBNull.Value ? "" : row["Fatura_Tarihi"].ToString();
                    item.MalTeslimTarihi = row["MalTeslimTarihi"] == DBNull.Value ? "" : row["MalTeslimTarihi"].ToString();
                    item.KapanisTarihi = row["KapanisTarihi"] == DBNull.Value ? "" : row["KapanisTarihi"].ToString();
                    item.BulunduguYer = row["BulunduguYer"] == DBNull.Value ? "" : row["BulunduguYer"].ToString();
                    item.Teminat_Tipi = row["Teminat_Tipi"] == DBNull.Value ? "" : row["Teminat_Tipi"].ToString();
                    item.Teminat_RefNo = row["Teminat_RefNo"] == DBNull.Value ? "" : row["Teminat_RefNo"].ToString();
                    item.Teminat_Tutar = row["Teminat_Tutar"] == DBNull.Value ? "" : row["Teminat_Tutar"].ToString();
                    item.KalemSayisi = row["KalemSayisi"] == DBNull.Value ? "" : row["KalemSayisi"].ToString();
                    item.KonismentoNo = row["KonismentoNo"] == DBNull.Value ? "" : row["KonismentoNo"].ToString();
                    item.MusteriFaturaNo = row["MusteriFaturaNo"] == DBNull.Value ? "" : row["MusteriFaturaNo"].ToString();
                    item.MusteriFaturaTarihi = row["MusteriFaturaTarihi"] == DBNull.Value ? "" : row["MusteriFaturaTarihi"].ToString();
                    item.Ay = row["Ay"] == DBNull.Value ? "" : row["Ay"].ToString();
                    item.Firma = row["Firma"] == DBNull.Value ? "" : row["Firma"].ToString();
                    
                    items.Add(item);
            }
            oResponseModel.Data = items;                            
            return oResponseModel;
        }

        public static cResponseModel<cIhracatRaporu>? getIhracatRaporu(string firma_no, string tescil_tarihi_1, string tescil_tarihi_2, string kullanici, string parola, string udid)
        {
            cResponseModel<cIhracatRaporu> oResponseModel = new cResponseModel<cIhracatRaporu>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            List<cIhracatRaporu> items = new List<cIhracatRaporu>();

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            string sql = $@" exec Evrim_DB.dbo.emobil_IhracatRaporu {firma_no} , '{DateTime.Parse(tescil_tarihi_1)}' , '{DateTime.Parse(tescil_tarihi_2)}' ,'{kullanici}',  '{parola}' , '{udid}' ";

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
                    cIhracatRaporu item = new cIhracatRaporu();
                                                        
                    item.Gonderen = row["Gonderen"] == DBNull.Value ? "" : row["Gonderen"].ToString();
                    item.Dosya_No = row["Dosya_No"] == DBNull.Value ? "" : row["Dosya_No"].ToString();
                    item.Gonderen = row["Gonderen"] == DBNull.Value ? "" : row["Gonderen"].ToString();
                    item.Alici = row["Alici"] == DBNull.Value ? "" : row["Alici"].ToString();
                    item.Imalatci = row["Imalatci"] == DBNull.Value ? "" : row["Imalatci"].ToString();
                    item.GCBNo = row["GCBNo"] == DBNull.Value ? "" : row["GCBNo"].ToString();
                    item.GCB_Tarihi = row["GCB_Tarihi"] == DBNull.Value ? "" : row["GCB_Tarihi"].ToString();
                    item.Intac_Tarihi = row["Intac_Tarihi"] == DBNull.Value ? "" : row["Intac_Tarihi"].ToString();
                    item.Koli = row["Koli"] == DBNull.Value ? "" : row["Koli"].ToString();
                    item.Ulke = row["Ulke"] == DBNull.Value ? "" : row["Ulke"].ToString();
                    item.Gumruk = row["Gumruk"] == DBNull.Value ? "" : row["Gumruk"].ToString();
                    item.FOB = row["FOB"] == DBNull.Value ? "" : row["FOB"].ToString();
                    item.Fatura_Bedeli = row["Fatura_Bedeli"] == DBNull.Value ? "" : row["Fatura_Bedeli"].ToString();
                    item.DC = row["DC"] == DBNull.Value ? "" : row["DC"].ToString();
                    item.CIF = row["CIF"] == DBNull.Value ? "" : row["CIF"].ToString();
                    item.Nakliyeci = row["Nakliyeci"] == DBNull.Value ? "" : row["Nakliyeci"].ToString();
                    item.Kapanis_Tarihi = row["Kapanis_Tarihi"] == DBNull.Value ? "" : row["Kapanis_Tarihi"].ToString();
                    item.Teslim_Sekli = row["Teslim_Sekli"] == DBNull.Value ? "" : row["Teslim_Sekli"].ToString();
                    item.Odeme_Sekli = row["Odeme_Sekli"] == DBNull.Value ? "" : row["Odeme_Sekli"].ToString();
                    item.Giren = row["Giren"] == DBNull.Value ? "" : row["Giren"].ToString();
                    item.M_Fat_No = row["M_Fat_No"] == DBNull.Value ? "" : row["M_Fat_No"].ToString();
                    item.M_Fat_Tarihi = row["M_Fat_Tarihi"] == DBNull.Value ? "" : row["M_Fat_Tarihi"].ToString();
                    item.Net_Kilo = row["Net_Kilo"] == DBNull.Value ? "" : row["Net_Kilo"].ToString();
                    item.Brut_Kilo = row["Brut_Kilo"] == DBNull.Value ? "" : row["Brut_Kilo"].ToString();
                    item.Miktar = row["Miktar"] == DBNull.Value ? "" : row["Miktar"].ToString();
                    item.Muayene_Memuru = row["Muayene_Memuru"] == DBNull.Value ? "" : row["Muayene_Memuru"].ToString();
                    item.Referans = row["Referans"] == DBNull.Value ? "" : row["Referans"].ToString();
                    item.ManifestoNo = row["ManifestoNo"] == DBNull.Value ? "" : row["ManifestoNo"].ToString();
                    item.Araci_Banka = row["Araci_Banka"] == DBNull.Value ? "" : row["Araci_Banka"].ToString();
                    item.Rejim = row["Rejim"] == DBNull.Value ? "" : row["Rejim"].ToString();
                    item.Telafi_Vergi = row["Telafi_Vergi"] == DBNull.Value ? "" : row["Telafi_Vergi"].ToString();
                    item.Tasit_Cinsi = row["Tasit_Cinsi"] == DBNull.Value ? "" : row["Tasit_Cinsi"].ToString();
                    item.Birlik = row["Birlik"] == DBNull.Value ? "" : row["Birlik"].ToString();
                    item.Aidat_Parasi = row["Aidat_Parasi"] == DBNull.Value ? "" : row["Aidat_Parasi"].ToString();
                    item.Crypto_Number = row["Crypto_Number"] == DBNull.Value ? "" : row["Crypto_Number"].ToString();
                    item.BeyannameSiraNo = row["BeyannameSiraNo"] == DBNull.Value ? "" : row["BeyannameSiraNo"].ToString();
                    item.MenseSahSiraNo = row["MenseSahSiraNo"] == DBNull.Value ? "" : row["MenseSahSiraNo"].ToString();
                    item.ATRSiraNo = row["ATRSiraNo"] == DBNull.Value ? "" : row["ATRSiraNo"].ToString();
                    item.EURO1SiraNo = row["EURO1SiraNo"] == DBNull.Value ? "" : row["EURO1SiraNo"].ToString();
                    item.EUROMEDSiraNo = row["EUROMEDSiraNo"] == DBNull.Value ? "" : row["EUROMEDSiraNo"].ToString();
                    item.FORMASiraNo = row["FORMASiraNo"] == DBNull.Value ? "" : row["FORMASiraNo"].ToString();
                    item.Fatura_No = row["Fatura_No"] == DBNull.Value ? "" : row["Fatura_No"].ToString();
                    item.Fatura_Tarihi = row["Fatura_Tarihi"] == DBNull.Value ? "" : row["Fatura_Tarihi"].ToString();
                    item.TirKntynrSayisi = row["TirKntynrSayisi"] == DBNull.Value ? "" : row["TirKntynrSayisi"].ToString();
                    item.Istatistiki_Kiymet = row["Istatistiki_Kiymet"] == DBNull.Value ? "" : row["Istatistiki_Kiymet"].ToString();
                    item.Acilis_Tarihi = row["Acilis_Tarihi"] == DBNull.Value ? "" : row["Acilis_Tarihi"].ToString();
                    item.Vergi_Turu = row["Vergi_Turu"] == DBNull.Value ? "" : row["Vergi_Turu"].ToString();
                    item.Vergi_Tutari = row["Vergi_Tutari"] == DBNull.Value ? "" : row["Vergi_Tutari"].ToString();
                    item.IsTakipKodu = row["IsTakipKodu"] == DBNull.Value ? "" : row["IsTakipKodu"].ToString();
                    item.CikisVasitasi = row["CikisVasitasi"] == DBNull.Value ? "" : row["CikisVasitasi"].ToString();
                    item.KonteynerNo = row["KonteynerNo"] == DBNull.Value ? "" : row["KonteynerNo"].ToString();
                    item.Ay = row["Ay"] == DBNull.Value ? "" : row["Ay"].ToString();
                    item.Firma = row["Firma"] == DBNull.Value ? "" : row["Firma"].ToString();
                    
                    items.Add(item);
            }
            oResponseModel.Data = items;                            
            return oResponseModel;
        }
    }
}
