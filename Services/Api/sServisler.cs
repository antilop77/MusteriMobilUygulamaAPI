using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MmuAPI.Models;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.Api;

namespace MusteriMobilUygulamaAPI.Services.Api
{
    public class sServisler
    {
        public static cResponseModel<cFaturaSorgu>? getFaturaSorgu(string FirmaNo, string Kullanici, string Parola, string IlkTarih, string SonTarih, string UDID)
        {
            cResponseModel<cFaturaSorgu> oResponseModel = new cResponseModel<cFaturaSorgu>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            List<cFaturaSorgu> items = new List<cFaturaSorgu>();

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            string sql = $@" exec Evrim_DB.dbo.emobil_FaturaSorgu {FirmaNo}, {Kullanici}, {Parola}, '{DateTime.Parse(IlkTarih)}' , '{DateTime.Parse(SonTarih)}', '{UDID}' ";

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
                    cFaturaSorgu item = new cFaturaSorgu();
                                        
                    item.Tip = row["Tip"] == DBNull.Value ? "" : row["Tip"].ToString();
                    item.Faturano = row["Faturano"] == DBNull.Value ? "" : row["Faturano"].ToString();
                    item.Fatura_Tarihi = row["Fatura_Tarihi"] == DBNull.Value ? "" : row["Fatura_Tarihi"].ToString();
                    item.FaturaToplami = row["FaturaToplami"] == DBNull.Value ? "" : row["FaturaToplami"].ToString();
                    item.FaturaKur = row["FaturaKur"] == DBNull.Value ? "" : row["FaturaKur"].ToString();
                    item.Odeme = row["Odeme"] == DBNull.Value ? "" : row["Odeme"].ToString();
                    
                    items.Add(item);
            }
            oResponseModel.Data = items;                            
            return oResponseModel;
        }        

        public static cResponseModel<cFaturaNoSorgu>? getFaturaNoSorgu(string FirmaNo, string Kullanici, string Parola, string Tip, string FaturaNo, string Yil, string UDID)
        {
            cResponseModel<cFaturaNoSorgu> oResponseModel = new cResponseModel<cFaturaNoSorgu>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            List<cFaturaNoSorgu> items = new List<cFaturaNoSorgu>();

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            string sql = $@" exec Evrim_DB.dbo.emobil_FaturaNoSorgu {FirmaNo}, {Kullanici}, {Parola}, '{Tip}' , '{FaturaNo}', '{Yil}', '{UDID}' ";

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
                    cFaturaNoSorgu item = new cFaturaNoSorgu();
                                        
                    item.Faturano = row["Faturano"] == DBNull.Value ? "" : row["Faturano"].ToString();
                    item.Fatura_Tarihi = row["Fatura_Tarihi"] == DBNull.Value ? "" : row["Fatura_Tarihi"].ToString();
                    item.Aciklama = row["Aciklama"] == DBNull.Value ? "" : row["Aciklama"].ToString();
                    item.Tutar = row["Tutar"] == DBNull.Value ? "" : row["Tutar"].ToString();
                    item.KDV = row["KDV"] == DBNull.Value ? "" : row["KDV"].ToString();                    
                    item.FaturaKur = row["FaturaKur"] == DBNull.Value ? "" : row["FaturaKur"].ToString();                    

                    items.Add(item);
            }
            oResponseModel.Data = items;                            
            return oResponseModel;
        }

        public static cResponseModel<cMobilLogin2>? getMobilLogin2(string firma_no, string kullanici, string parola, string udid)
        {
            cResponseModel<cMobilLogin2> oResponseModel = new cResponseModel<cMobilLogin2>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            List<cMobilLogin2> items = new List<cMobilLogin2>();

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            string sql = $@" exec Evrim_DB.dbo.emobil_login_2 {kullanici}, {parola}, '{firma_no}', '{udid}' ";

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
                    cMobilLogin2 item = new cMobilLogin2();
                                        
                    item.Grup = row["Grup"] == DBNull.Value ? "" : row["Grup"].ToString();
                    item.Baslik = row["Baslik"] == DBNull.Value ? "" : row["Baslik"].ToString();
                    item.Deger = row["Deger"] == DBNull.Value ? "" : row["Deger"].ToString();

                    items.Add(item);
            }
            oResponseModel.Data = items;                            
            return oResponseModel;
        } 

        public static cResponseModel<cBakiyeBorc>? getBakiyeBorc(string firma_no, string kullanici, string parola, string udid)
        {
            cResponseModel<cBakiyeBorc> oResponseModel = new cResponseModel<cBakiyeBorc>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            List<cBakiyeBorc> items = new List<cBakiyeBorc>();

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            string sql = $@" exec Evrim_DB.dbo.emobil_Bakiye_Borc {firma_no}, '{kullanici}', '{parola}', '{udid}' ";

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
                    cBakiyeBorc item = new cBakiyeBorc();
                    
                    item.Doviz = row["Doviz"] == DBNull.Value ? "" : row["Doviz"].ToString();
                    item.Bakiye = row["Bakiye"] == DBNull.Value ? "" : row["Bakiye"].ToString();
                    item.VadesiGecmis = row["VadesiGecmis"] == DBNull.Value ? "" : row["VadesiGecmis"].ToString();
                    items.Add(item);
            }
            oResponseModel.Data = items;                            
            return oResponseModel;
        }

        public static cResponseModel<cEmobilIslemAdet>? getEmobilIslemAdet(string firma_no, string tip, string kullanici, string parola, string tarih1, string tarih2, string udid)
        {
            cResponseModel<cEmobilIslemAdet> oResponseModel = new cResponseModel<cEmobilIslemAdet>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            List<cEmobilIslemAdet> items = new List<cEmobilIslemAdet>();

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            string sql = $@" exec Evrim_DB.dbo.emobil_IslemAdet {firma_no}, '{tip}', '{kullanici}', '{parola}', '{DateTime.Parse(tarih1)}', '{DateTime.Parse(tarih2)}', '{udid}' ";

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
                    cEmobilIslemAdet item = new cEmobilIslemAdet();
                                        
                    item.Yil = row["Yil"] == DBNull.Value ? "" : row["Yil"].ToString();
                    item.Ay = row["Ay"] == DBNull.Value ? "" : row["Ay"].ToString();
                    item.Gumruk = row["Gumruk"] == DBNull.Value ? "" : row["Gumruk"].ToString();
                    item.Ulke = row["Ulke"] == DBNull.Value ? "" : row["Ulke"].ToString();
                    item.Adet = row["Adet"] == DBNull.Value ? "" : row["Adet"].ToString();
                    item.IstatistikiKiymet_USD = row["IstatistikiKiymet_USD"] == DBNull.Value ? "" : row["IstatistikiKiymet_USD"].ToString();
                    item.MalBedeliSatis = row["MalBedeliSatis"] == DBNull.Value ? "" : row["MalBedeliSatis"].ToString();
                    item.Sigorta = row["Sigorta"] == DBNull.Value ? "" : row["Sigorta"].ToString();
                    item.Navlun = row["Navlun"] == DBNull.Value ? "" : row["Navlun"].ToString();
                    item.CIFDiger = row["CIFDiger"] == DBNull.Value ? "" : row["CIFDiger"].ToString();
                    item.GVMatrahi = row["GVMatrahi"] == DBNull.Value ? "" : row["GVMatrahi"].ToString();
                    item.GV = row["GV"] == DBNull.Value ? "" : row["GV"].ToString();
                    item.DV = row["DV"] == DBNull.Value ? "" : row["DV"].ToString();
                    item.DigerVergi = row["DigerVergi"] == DBNull.Value ? "" : row["DigerVergi"].ToString();
                    item.Ardiye = row["Ardiye"] == DBNull.Value ? "" : row["Ardiye"].ToString();
                    item.Tahliye = row["Tahliye"] == DBNull.Value ? "" : row["Tahliye"].ToString();
                    item.BankaKomisyonu = row["BankaKomisyonu"] == DBNull.Value ? "" : row["BankaKomisyonu"].ToString();
                    item.Diger1 = row["Diger1"] == DBNull.Value ? "" : row["Diger1"].ToString();
                    item.Diger2 = row["Diger2"] == DBNull.Value ? "" : row["Diger2"].ToString();
                    item.KKDF = row["KKDF"] == DBNull.Value ? "" : row["KKDF"].ToString();
                    item.KulturFonu = row["KulturFonu"] == DBNull.Value ? "" : row["KulturFonu"].ToString();
                    item.KDV = row["KDV"] == DBNull.Value ? "" : row["KDV"].ToString();
                    item.KDVMatrahi = row["KDVMatrahi"] == DBNull.Value ? "" : row["KDVMatrahi"].ToString();
                    item.PesinDeger = row["PesinDeger"] == DBNull.Value ? "" : row["PesinDeger"].ToString();
                    item.TeminatDeger = row["TeminatDeger"] == DBNull.Value ? "" : row["TeminatDeger"].ToString();
                    item.Toplamvergi = row["Toplamvergi"] == DBNull.Value ? "" : row["Toplamvergi"].ToString();

                    items.Add(item);
            }
            oResponseModel.Data = items;                            
            return oResponseModel;
        }

        public static cResponseModel<cBeyannameDetay>? getBeyannameDetay(string firma_no, string tip, string dosyano)
        {
            cResponseModel<cBeyannameDetay> oResponseModel = new cResponseModel<cBeyannameDetay>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            List<cBeyannameDetay> items = new List<cBeyannameDetay>();

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            string sql = $@" exec Evrim_DB.dbo.emobil_BeyannameDetay {firma_no}, '{tip}', '{dosyano}' ";

            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader readerForLikes = command.ExecuteReader();
            DataTable dataTableForLikes = new DataTable();
            dataTableForLikes.Load(readerForLikes);
            connection.Close();            
            
            foreach (DataRow row in dataTableForLikes.Rows)
            {
                    cBeyannameDetay item = new cBeyannameDetay();
                    
                    item.Baslik = row["Baslik"] == DBNull.Value ? "" : row["Baslik"].ToString();
                    item.Deger = row["Deger"] == DBNull.Value ? "" : row["Deger"].ToString();
                    
                    items.Add(item);
            }
            oResponseModel.Data = items;                            
            return oResponseModel;
        }

        public static cResponseModel<cBeyannameDetayKalem>? getBeyannameDetayKalem(string firma_no, string tip, string dosyano)
        {
            cResponseModel<cBeyannameDetayKalem> oResponseModel = new cResponseModel<cBeyannameDetayKalem>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            List<cBeyannameDetayKalem> items = new List<cBeyannameDetayKalem>();

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            string sql = $@" exec Evrim_DB.dbo.emobil_BeyannameDetayKalem {firma_no}, '{tip}', '{dosyano}' ";

            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader readerForLikes = command.ExecuteReader();
            DataTable dataTableForLikes = new DataTable();
            dataTableForLikes.Load(readerForLikes);
            connection.Close();            
            
            foreach (DataRow row in dataTableForLikes.Rows)
            {
                    cBeyannameDetayKalem item = new cBeyannameDetayKalem();
                    
                    item.SiraNo = row["SiraNo"] == DBNull.Value ? "" : row["SiraNo"].ToString();
                    item.Gtip_No = row["Gtip_No"] == DBNull.Value ? "" : row["Gtip_No"].ToString();
                    item.TicariTanim = row["TicariTanim"] == DBNull.Value ? "" : row["TicariTanim"].ToString();
                    item.MenseiUlke = row["MenseiUlke"] == DBNull.Value ? "" : row["MenseiUlke"].ToString();
                    item.Miktar = row["Miktar"] == DBNull.Value ? "" : row["Miktar"].ToString();
                    item.KalemFiyat = row["KalemFiyat"] == DBNull.Value ? "" : row["KalemFiyat"].ToString();
                    item.IstatistikiKiymet = row["IstatistikiKiymet"] == DBNull.Value ? "" : row["IstatistikiKiymet"].ToString();
                    item.Navlun = row["Navlun"] == DBNull.Value ? "" : row["Navlun"].ToString();
                    item.Sigorta = row["Sigorta"] == DBNull.Value ? "" : row["Sigorta"].ToString();
                    item.CIFDiger = row["CIFDiger"] == DBNull.Value ? "" : row["CIFDiger"].ToString();
                    item.Ardiye = row["Ardiye"] == DBNull.Value ? "" : row["Ardiye"].ToString();
                    item.Pul_Ord_KF = row["Pul_Ord_KF"] == DBNull.Value ? "" : row["Pul_Ord_KF"].ToString();
                    item.BankaKom = row["BankaKom"] == DBNull.Value ? "" : row["BankaKom"].ToString();
                    item.CIFSatis = row["CIFSatis"] == DBNull.Value ? "" : row["CIFSatis"].ToString();
                    item.Diger1 = row["Diger1"] == DBNull.Value ? "" : row["Diger1"].ToString();
                    item.Diger2 = row["Diger2"] == DBNull.Value ? "" : row["Diger2"].ToString();
                    item.KulturFonu = row["KulturFonu"] == DBNull.Value ? "" : row["KulturFonu"].ToString();
                    item.KKDF = row["KKDF"] == DBNull.Value ? "" : row["KKDF"].ToString();
                    item.VergiGVMatrahi = row["VergiGVMatrahi"] == DBNull.Value ? "" : row["VergiGVMatrahi"].ToString();
                    item.VergiGVOrani = row["VergiGVOrani"] == DBNull.Value ? "" : row["VergiGVOrani"].ToString();
                    item.VergiGV = row["VergiGV"] == DBNull.Value ? "" : row["VergiGV"].ToString();
                    item.VergiKDVMatrahi = row["VergiKDVMatrahi"] == DBNull.Value ? "" : row["VergiKDVMatrahi"].ToString();
                    item.VergiKDVOrani = row["VergiKDVOrani"] == DBNull.Value ? "" : row["VergiKDVOrani"].ToString();
                    item.VergiKDV = row["VergiKDV"] == DBNull.Value ? "" : row["VergiKDV"].ToString();
                    item.VergiDamga = row["VergiDamga"] == DBNull.Value ? "" : row["VergiDamga"].ToString();
                    item.VergiAntiDamping = row["VergiAntiDamping"] == DBNull.Value ? "" : row["VergiAntiDamping"].ToString();
                    item.VergiAntiDampingOrani = row["VergiAntiDampingOrani"] == DBNull.Value ? "" : row["VergiAntiDampingOrani"].ToString();
                    item.VergiEkMaliYukumluluk = row["VergiEkMaliYukumluluk"] == DBNull.Value ? "" : row["VergiEkMaliYukumluluk"].ToString();
                    item.VergiEkMaliYukumlulukOrani = row["VergiEkMaliYukumlulukOrani"] == DBNull.Value ? "" : row["VergiEkMaliYukumlulukOrani"].ToString();
                    item.VergiOzelTuketimListeIV = row["VergiOzelTuketimListeIV"] == DBNull.Value ? "" : row["VergiOzelTuketimListeIV"].ToString();
                    item.VergiOzelTuketimListeIVOrani = row["VergiOzelTuketimListeIVOrani"] == DBNull.Value ? "" : row["VergiOzelTuketimListeIVOrani"].ToString();
                    item.OlcuBirimi = row["OlcuBirimi"] == DBNull.Value ? "" : row["OlcuBirimi"].ToString();
                    item.OlcuMiktar = row["OlcuMiktar"] == DBNull.Value ? "" : row["OlcuMiktar"].ToString();
                    item.TesvikKalemNo = row["TesvikKalemNo"] == DBNull.Value ? "" : row["TesvikKalemNo"].ToString();
                    item.MC = row["MC"] == DBNull.Value ? "" : row["MC"].ToString();
                 
                    items.Add(item);
            }
            oResponseModel.Data = items;                            
            return oResponseModel;
        }

        public static cResponseModel<cBeyannameMasraf>? getBeyannameMasraf(string firma_no, string tip, string dosyano)
        {
            cResponseModel<cBeyannameMasraf> oResponseModel = new cResponseModel<cBeyannameMasraf>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            List<cBeyannameMasraf> items = new List<cBeyannameMasraf>();

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            string sql = $@" exec Evrim_DB.dbo.emobil_BeyannameMasraf {firma_no}, '{tip}', '{dosyano}' ";

            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader readerForLikes = command.ExecuteReader();
            DataTable dataTableForLikes = new DataTable();
            dataTableForLikes.Load(readerForLikes);
            connection.Close();            
            
            foreach (DataRow row in dataTableForLikes.Rows)
            {
                    cBeyannameMasraf item = new cBeyannameMasraf();
                    
                    item.Dosya_no = row["Dosya_no"] == DBNull.Value ? "" : row["Dosya_no"].ToString();
                    item.MalBedeliSatis = row["MalBedeliSatis"] == DBNull.Value ? "" : row["MalBedeliSatis"].ToString();
                    item.Sigorta = row["Sigorta"] == DBNull.Value ? "" : row["Sigorta"].ToString();
                    item.Navlun = row["Navlun"] == DBNull.Value ? "" : row["Navlun"].ToString();
                    item.CIFDiger = row["CIFDiger"] == DBNull.Value ? "" : row["CIFDiger"].ToString();
                    item.GVMatrahi = row["GVMatrahi"] == DBNull.Value ? "" : row["GVMatrahi"].ToString();
                    item.GV = row["GV"] == DBNull.Value ? "" : row["GV"].ToString();
                    item.DV = row["DV"] == DBNull.Value ? "" : row["DV"].ToString();
                    item.DigerVergi = row["DigerVergi"] == DBNull.Value ? "" : row["DigerVergi"].ToString();
                    item.Ardiye = row["Ardiye"] == DBNull.Value ? "" : row["Ardiye"].ToString();
                    item.Tahliye = row["Tahliye"] == DBNull.Value ? "" : row["Tahliye"].ToString();
                    item.BankaKomisyonu = row["BankaKomisyonu"] == DBNull.Value ? "" : row["BankaKomisyonu"].ToString();
                    item.Diger1 = row["Diger1"] == DBNull.Value ? "" : row["Diger1"].ToString();
                    item.Diger2 = row["Diger2"] == DBNull.Value ? "" : row["Diger2"].ToString();
                    item.KKDF = row["KKDF"] == DBNull.Value ? "" : row["KKDF"].ToString();
                    item.KulturFonu = row["KulturFonu"] == DBNull.Value ? "" : row["KulturFonu"].ToString();
                    item.KDVMatrahi = row["KDVMatrahi"] == DBNull.Value ? "" : row["KDVMatrahi"].ToString();
                    item.KDV = row["KDV"] == DBNull.Value ? "" : row["KDV"].ToString();
                    item.PesinDeger = row["PesinDeger"] == DBNull.Value ? "" : row["PesinDeger"].ToString();
                    item.TeminatDeger = row["TeminatDeger"] == DBNull.Value ? "" : row["TeminatDeger"].ToString();
                    item.Toplamvergi = row["Toplamvergi"] == DBNull.Value ? "" : row["Toplamvergi"].ToString();
                    item.IstatistikiKiymet = row["IstatistikiKiymet"] == DBNull.Value ? "" : row["IstatistikiKiymet"].ToString();
                    items.Add(item);
            }
            oResponseModel.Data = items;                            
            return oResponseModel;
        }
    }
}
