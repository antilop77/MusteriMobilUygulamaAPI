using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.Intranet;
using System.Data.SqlClient;

namespace MusteriMobilUygulamaAPI.Services.Intranet
{
    public class sIntranet
    {
        public static cResponseModel<cRehberDepartmanListesi> getRehberDepartmanListesi()
        {            
            List<cRehberDepartmanListesi> items = new List<cRehberDepartmanListesi>();
            cResponseModel<cRehberDepartmanListesi> oResponseModel = new cResponseModel<cRehberDepartmanListesi>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionStringForKurumsal;
            
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                
                string sql = $@" exec Intranet.dbo.RehberDepartmanListesi ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cRehberDepartmanListesi item = new cRehberDepartmanListesi();
                            item.DepartmanAdi = reader["DepartmanAdi"].ToString();
                            items.Add(item);
                        }
                    }
                    
                }
                connection.Close();
            }
            
            oResponseModel.Data = items;
            return oResponseModel;
        }

        public static cResponseModel<cRehberMusavirKarneListesi> getRehberMusavirKarneListesi()
        {            
            List<cRehberMusavirKarneListesi> items = new List<cRehberMusavirKarneListesi>();
            cResponseModel<cRehberMusavirKarneListesi> oResponseModel = new cResponseModel<cRehberMusavirKarneListesi>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionStringForKurumsal;
            
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                
                string sql = $@" select * from Intranet.dbo.MusavirKarne  ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cRehberMusavirKarneListesi item = new cRehberMusavirKarneListesi();
                            item.Id = reader["Id"].ToString();
                            item.KarneAd = reader["KarneAd"].ToString();
                            
                            items.Add(item);
                        }
                    }
                    
                }
                connection.Close();
            }
            
            oResponseModel.Data = items;
            return oResponseModel;
        }

        public static cResponseModel<cRehberGorevListesi> getRehberGorevListesi()
        {            
            List<cRehberGorevListesi> items = new List<cRehberGorevListesi>();
            cResponseModel<cRehberGorevListesi> oResponseModel = new cResponseModel<cRehberGorevListesi>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionStringForKurumsal;
            
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                
                string sql = $@" exec Intranet.dbo.RehberGorevListesi  ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cRehberGorevListesi item = new cRehberGorevListesi();
                            item.Gorevi = reader["Gorevi"].ToString();
                            
                            items.Add(item);
                        }
                    }
                    
                }
                connection.Close();
            }
            
            oResponseModel.Data = items;
            return oResponseModel;
        }

        public static cResponseModel<cRehberSorgulama> getRehberSorgulama(string adsoyad, string gorev, string departman, string dahili, string? kisaKod, string? BolgeTanimi, string? SubeTanimi)
        {            
            List<cRehberSorgulama> items = new List<cRehberSorgulama>();
            cResponseModel<cRehberSorgulama> oResponseModel = new cResponseModel<cRehberSorgulama>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionStringForKurumsal;

            adsoyad = adsoyad == null ? "null" : adsoyad;
            gorev = gorev == null ? "null" : gorev;
            departman = departman == null ? "null" : departman;
            dahili = dahili == null ? "null" : dahili;
            BolgeTanimi = BolgeTanimi == null ? "null" : "'" + BolgeTanimi + "'";
            SubeTanimi = SubeTanimi == null ? "null" : "'" + SubeTanimi + "'";
            kisaKod = kisaKod == null ? "null" : kisaKod;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                
                string sql = $@" exec Intranet.dbo.RehberSorgu {adsoyad}, {departman}, {gorev}, {dahili}, {BolgeTanimi}, {SubeTanimi}, {kisaKod} ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cRehberSorgulama item = new cRehberSorgulama();
                            item.TelRehberID = reader["TelRehberID"].ToString();
                            item.Sirket = reader["Sirket"].ToString();
                            item.AdSoyad = reader["AdSoyad"].ToString();
                            item.DepartmanAdi = reader["DepartmanAdi"].ToString();
                            item.Gorevi = reader["Gorevi"].ToString();
                            item.Yonetici = reader["Yonetici"].ToString();
                            item.Sektor = reader["Sektor"].ToString();
                            item.Dahili = reader["Dahili"].ToString();
                            item.KisaKod = reader["KisaKod"].ToString();
                            item.KisaUzun = reader["KisaUzun"].ToString();
                            item.TcKimlikNo = reader["TcKimlikNo"].ToString();
                            item.bolgeTanimi = reader["bolgeTanimi"].ToString();
                            item.subeTanimi = reader["subeTanimi"].ToString();

                            items.Add(item);
                        }
                    }
                    
                }
                connection.Close();
            }
            
            oResponseModel.Data = items;
            return oResponseModel;
        }

        public static cResponseModel<cMusavirKarneListesi> getMusavirKarneListesi(string adsoyad, string gorev, string departman, string dahili, string? kisaKod, string? BolgeTanimi, string? SubeTanimi)
        {            
            List<cMusavirKarneListesi> items = new List<cMusavirKarneListesi>();
            cResponseModel<cMusavirKarneListesi> oResponseModel = new cResponseModel<cMusavirKarneListesi>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;
            
            adsoyad = adsoyad == null ? "null" : adsoyad;
            gorev = gorev == null ? "null" : gorev;
            departman = departman == null ? "null" : departman;
            dahili = dahili == null ? "null" : dahili;            
            kisaKod = kisaKod == null ? "null" : kisaKod;
            BolgeTanimi = BolgeTanimi == null ? "null" : "'" + BolgeTanimi + "'";
            SubeTanimi = SubeTanimi == null ? "null" : "'" + SubeTanimi + "'";

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                
                string sql = $@" exec Evrim_DB.dbo.pGetIntranetRehberMusavirKarneListesi {adsoyad}, {departman}, {gorev}, {dahili}, {kisaKod}, {BolgeTanimi}, {SubeTanimi} ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cMusavirKarneListesi item = new cMusavirKarneListesi();
                            item.TelRehberID = reader["TelRehberID"].ToString();
                            item.Sirket = reader["Sirket"].ToString();
                            item.AdSoyad = reader["AdSoyad"].ToString();
                            item.bolgeTanimi = reader["bolgeTanimi"].ToString();
                            item.subeTanimi = reader["subeTanimi"].ToString();
                            item.DepartmanAdi = reader["DepartmanAdi"].ToString();
                            item.Gorevi = reader["Gorevi"].ToString();
                            item.Yonetici = reader["Yonetici"].ToString();
                            item.Sektor = reader["Sektor"].ToString();
                            item.Dahili = reader["Dahili"].ToString();
                            item.KisaKod = reader["KisaKod"].ToString();
                            item.TCKimlikNo = reader["TCKimlikNo"].ToString();
                            item.KisaUzun = reader["KisaUzun"].ToString();
                            
                            items.Add(item);
                        }
                    }
                    
                }
                connection.Close();
            }
            
            oResponseModel.Data = items;
            return oResponseModel;
        }

        public static cResponseModel<cBolgeler> getBolgeler()
        {            
            List<cBolgeler> items = new List<cBolgeler>();
            cResponseModel<cBolgeler> oResponseModel = new cResponseModel<cBolgeler>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;
            
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                
                string sql = $@"select distinct bolgeTanimi 
                                from UGM_ERP.dbo.PersonelDetayBilgileri 
                                where 1=1 
                                and bolgeTanimi is not null 
                                and bolgeTanimi != '-' 
                                order by bolgeTanimi ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cBolgeler item = new cBolgeler();
                            item.bolgeTanimi = reader["bolgeTanimi"].ToString();
                            
                            items.Add(item);
                        }
                    }
                    
                }
                connection.Close();
            }
            
            oResponseModel.Data = items;
            return oResponseModel;
        }

        public static cResponseModel<cSubeler> getSubeler()
        {            
            List<cSubeler> items = new List<cSubeler>();
            cResponseModel<cSubeler> oResponseModel = new cResponseModel<cSubeler>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;
            
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                
                string sql = $@"select distinct subeTanimi
                                from UGM_ERP.dbo.PersonelDetayBilgileri
                                where 1=1
                                and subeTanimi is not null
                                and subeTanimi != '-'
                                order by subeTanimi ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cSubeler item = new cSubeler();
                            item.subeTanimi = reader["subeTanimi"].ToString();
                            
                            items.Add(item);
                        }
                    }
                    
                }
                connection.Close();
            }
            
            oResponseModel.Data = items;
            return oResponseModel;
        }
    }
}
