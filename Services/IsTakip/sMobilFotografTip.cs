using System.Data.SqlClient;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.IsTakip;

namespace MusteriMobilUygulamaAPI.Services.IsTakip
{
    public static class sMobilFotografTip
    {
        public static cResponseModel<cMobilFotografTip> get()
        {
            //int y=0, x = 1/y;
            List<cMobilFotografTip> items = new List<cMobilFotografTip>();
            cResponseModel<cMobilFotografTip> oResponseModel = new cResponseModel<cMobilFotografTip>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = @" SELECT 
                                    ID,
                                    MOBIL_GORUNUM_ISMI,
                                    IS_TAKIP_KODU,
                                    IS_TAKIP_ACIKLAMASI,
                                    ARSIV_DOSYA_SINIFI,
                                    ARSIV_DOKUMAN_ADI
                                FROM UGM_ERP.dbo.MOBIL_FOTOGRAF_TIP ";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cMobilFotografTip item = new cMobilFotografTip();
                            item.id = reader.GetInt32(0);
                            item.mobil_gorunum_ismi = reader.GetString(1);
                            item.is_takip_kodu = reader.GetString(2);
                            item.is_takip_aciklamasi = reader.GetString(3);
                            item.arsiv_dosya_sinifi = reader.GetString(4);
                            item.arsiv_dokuman_adi = reader.GetString(5);

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
