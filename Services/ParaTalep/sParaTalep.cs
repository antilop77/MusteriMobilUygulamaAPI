using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models.ParaTalep;
using System.Data.SqlClient;
using System.Data;
using MmuAPI.Models;
using MusteriMobilUygulamaAPI.Models;
using System.Globalization;

namespace MusteriMobilUygulamaAPI.Services.ParaTalep
{
    public class sParaTalep
    {
        public static cResponseModel<int> getParaTalepOnaylama(int ID, DateTime onayTarihi, string onaySaati, int detayNo, int? userID)
        {
            //int y=0, xx = 1/y;
            cResponseModel<int> oResponseModel = new cResponseModel<int>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = $@"select 
                                Adi_Soyadi AdSoyad, 
                                ISNULL(TCKimlik,'') tckimlik,
                                GorevTanimi FirmaUnvan,
                                Kodu from users with(nolock) where Users_ID = {userID}";

                cUserInfo oUserInfo; // = new cUserInfo();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        oUserInfo = dataTable.AsEnumerable().Select(m => new cUserInfo()
                        {
                            kodu = m.Field<string?>("Kodu")
                        }).FirstOrDefault();
                    }
                }

                sql = $@"Set Dateformat dmy Update paratalep set 
                        OnayVeren='{oUserInfo?.kodu}',  
                        OnayTarih='{onayTarihi.ToString("dd.MM.yyyy")}',  
                        OnaySaati='{onaySaati}',  
                        Kulkod='{oUserInfo?.kodu}',       
                        OzelNot=Case when isnull(OzelNot,'')='' then '.' else OzelNot end,
                        EMobilOnay=1
                        Where 1=1 
                        and ID = {ID} 
                        and (OnayVeren IS NULL OR OnayVeren = '') ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    int x = command.ExecuteNonQuery();
                    if (x > 0)
                    {
                        oResponseModel.errorModel.ErrorMessage = "Talep onaylandı.";
                    }
                    else
                    {
                        oResponseModel.errorModel.ErrorCode = -1;
                        oResponseModel.errorModel.ErrorMessage = "Onaylanacak kayıt bulunamadı.";
                    }
                }
                connection.Close();
            }

            return oResponseModel;
        }

        public static cResponseModel<int> postParaTalepGuncelleme(int ID, decimal tutar, int? userID)
        {
            //int y=0, xx = 1/y;
            cResponseModel<int> oResponseModel = new cResponseModel<int>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                
                string sql = $@"update paratalep
                                set Tutar = @p1
                                where 1=1
                                and (OnayVeren IS NULL OR OnayVeren = '') 
                                and ID = @p2 ";
                
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@p1", tutar);
                    command.Parameters.AddWithValue("@p2", ID);
                    int x = command.ExecuteNonQuery();
                
                    if (x > 0)
                    {
                        oResponseModel.errorModel.ErrorMessage = "Talep güncellendi.";
                    }
                    else
                    {
                        oResponseModel.errorModel.ErrorCode = -1;
                        oResponseModel.errorModel.ErrorMessage = "Güncellenecek kayıt bulunamadı.";
                    }
                }
                connection.Close();
            }

            return oResponseModel;
        }

        public static cResponseModel<int> postParaTalepSilme(int ID)
        {
            //int y=0, xx = 1/y;
            cResponseModel<int> oResponseModel = new cResponseModel<int>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                
                string sql = $@" DELETE FROM paratalep 
                                where 1=1
                                and (OnayVeren IS NULL OR OnayVeren = '')  
                                and ID = @p1 ";
                
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@p1", ID);
                    int x = command.ExecuteNonQuery();

                    if (x > 0)
                    {
                        oResponseModel.errorModel.ErrorMessage = "Talep silindi.";
                    }
                    else
                    {
                        oResponseModel.errorModel.ErrorCode = -1;
                        oResponseModel.errorModel.ErrorMessage = "Silinecek kayıt bulunamadı.";
                    }
                }
                connection.Close();
            }

            return oResponseModel;
        }
    }
}
