using MusteriMobilUygulamaAPI.Models.Api;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Services.Api;
using System.Data.SqlClient;
using System.Data;
using MusteriMobilUygulamaAPI.Models.Intranet;
using System.Numerics;
using System.Globalization;
using MusteriMobilUygulamaAPI;
using System.Collections;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace MusteriMobilUygulamaAPI.Services.Intranet
{
    public class sHaftalikYemek
    {
        public static cResponseModel<cWeek> getHaftalikYemek()
        {            
            List<cWeek> items = new List<cWeek>();
            cResponseModel<cWeek> oResponseModel = new cResponseModel<cWeek>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionStringForKurumsal;

            List<cDayElement> oDays = new List<cDayElement>(); 
            int weekNum = 0;
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                CultureInfo ciCurr = CultureInfo.CurrentCulture;
                weekNum = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                
                string sql = $@" exec Intranet.dbo.YemekListesiWithIsoDate {weekNum} ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {                                     
                        while (reader.Read())
                        {        
                            cDayElement oDayElement = new cDayElement();
                            oDayElement.yemekid = (int)reader["YemekID"];
                            oDayElement.adi = reader["Adi"].ToString();
                            oDayElement.aciklama = reader["Aciklama"].ToString();
                            oDayElement.servistarihi = reader["ServisTarihi"].ToString();
                            oDayElement.yemektipid = (int)reader["YemekTipId"];
                            oDayElement.tipadi = reader["TipAdi"].ToString();
                            int dayOfWeek = (int) DateTime.Parse(reader["ServisTarihi"].ToString()).DayOfWeek;
                            oDays.Add(oDayElement);
                        }
                    }
                }
                connection.Close();
            }
            
            cWeek oWeek = new cWeek();            
            oWeek.hafta = weekNum;
            CultureInfo tr = new CultureInfo("tr-TR");
            DateTime dt = GetFirstDayOfWeek(2024, weekNum-1, tr);
            oWeek.tarih_bilgisi = dt.ToString("dd.MM.yyyy") + "-" + dt.AddDays(5).ToString("dd.MM.yyyy") ;
            int cntPazartesi = 0;
            int cntSali = 0;
            int cntCarsamba = 0;
            int cntPersembe = 0;
            int cntCuma = 0;
            int cntCumartesi = 0;
            
            foreach (cDayElement oDayElement in oDays)
            {
                int day = (int) DateTime.Parse(oDayElement?.servistarihi).DayOfWeek;
                switch (day)
                {
                    case 1:
                        oWeek.pazartesi[cntPazartesi++] = oDayElement;
                        break;
                    case 2:
                        oWeek.sali[cntSali++] = oDayElement;
                        break;
                    case 3:
                        oWeek.carsamba[cntCarsamba++] = oDayElement;
                        break;
                    case 4:
                        oWeek.persembe[cntPersembe++] = oDayElement;
                        break;
                    case 5:
                        oWeek.cuma[cntCuma++] = oDayElement;
                        break;
                    case 6:
                        oWeek.cumartesi[cntCumartesi++] = oDayElement;
                        break;
                }
                
            }      
            items.Add(oWeek);

            oResponseModel.Data = items;
            return oResponseModel;
        }

        public static DateTime GetFirstDayOfWeek(int year, int weekNumber,
        System.Globalization.CultureInfo culture)
        {
            System.Globalization.Calendar calendar = culture.Calendar;
            DateTime firstOfYear = new DateTime(year, 1, 1, calendar);
            DateTime targetDay = calendar.AddWeeks(firstOfYear, weekNumber);
            DayOfWeek firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;

            while (targetDay.DayOfWeek != firstDayOfWeek)
            {
                targetDay = targetDay.AddDays(-1);
            }

            return targetDay;
        }
    }
}
