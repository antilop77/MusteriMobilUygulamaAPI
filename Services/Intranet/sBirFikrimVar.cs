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
using System;

namespace MusteriMobilUygulamaAPI.Services.Intranet
{
    public class sBirFikrimVar
    {
        public static cResponseModel<int> postBirFikrimVar(string konu, string fikir, string username)
        {            
            List<int> items = new List<int>();
            cResponseModel<int> oResponseModel = new cResponseModel<int>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cCommon.ConnectionStringForKurumsal;
            string today = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            string userName = cCommon.getUgmUser(username);
            string mailBody = $@"<br/>{userName} adlı personel yeni bir fikir paylaşımında bulunmuştur.<br/><br/>
                <p><h4>Gelen Fikir:</h4></p>
                <p>{fikir}</p>
                <br/><br/>
                <p><strong>UGM Intranet - Bir fikrim Var Projesi</strong></p>
                <p><strong>Tarih:{today}</strong></p>";
            cSendMail oSendMail = new cSendMail();
            oSendMail.from = "bildirim@ugm.com.tr";
            oSendMail.to = "bildirim@ugm.com.tr";
            oSendMail.subject = $"{konu} - Bir Fikrim Var Projesi";
            oSendMail.cc = "bildirim@ugm.com.tr";
            oSendMail.bcc = "veyselsaltan@ugm.com.tr";
            oSendMail.body = mailBody;
            oSendMail.displayName = "";
            MailProcess.SendMailBildirim(oSendMail);
            //MailProcess.SendMailBildirim("bildirim@ugm.com.tr", "idrisboz@ugm.com.tr", "idrisboz@ugm.com.tr", "idrisboz@ugm.com.tr", $"{konu} - Bir Fikrim Var Projesi", mailBody, "");

            
            
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                
                string sql = $@" INSERT INTO Intranet.dbo.BirFikrimVar (Konu, Fikir, UserName, FullName, projetip) VALUES ( '{konu}', '{fikir}', '{username}', '{userName}', 'Fikir') ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    int cntInserted = command.ExecuteNonQuery();
                    if (cntInserted > 0)
                    {
                        items.Add(1);
                        oResponseModel.errorModel.ErrorMessage = "Fikir kaydı başarılı bir şekilde eklendi.";
                    }
                    else
                    {
                        oResponseModel.errorModel.ErrorCode = -1;
                        oResponseModel.errorModel.ErrorMessage = "Kayıt edilirken hata alındı ";
                    }
                }
                connection.Close();
            }
            
            oResponseModel.Data = items;
            return oResponseModel;
        }
    }
}
