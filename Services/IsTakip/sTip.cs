using System.Data.SqlClient;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.IsTakip;

namespace MusteriMobilUygulamaAPI.Services.IsTakip
{
    public static class sTip
    {

        public static cResponseModel<cTip> get()
        {
            //int y=0, x = 1/y;
            List<cTip> items = new List<cTip>();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionString;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = @" exec [Evrim_DB].[dbo].[eistakipmobil_tip] ";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cTip item = new cTip();
                            item.Tip = reader.GetString(0);
                            item.Aciklama = reader.GetString(1);

                            items.Add(item);
                        }
                    }
                }
                connection.Close();
            }

            cResponseModel<cTip> oResponseModel = new cResponseModel<cTip>();
            oResponseModel.Data = items;

            return oResponseModel;
        }
    }
}
