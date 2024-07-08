using System.Buffers.Text;
using System;
using System.Security.Principal;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data.SqlClient;
using MmuAPI.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using Microsoft.Win32.SafeHandles;
using SimpleImpersonation;
using System.IO;
using MusteriMobilUygulamaAPI.Models.Api;
using System.Data;
using MusteriMobilUygulamaAPI.Services.IsTakip;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Services;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using Microsoft.Extensions.Options;
using MmuAPI;

namespace MusteriMobilUygulamaAPI.Common
{
    public class cCommon
    {
        public static string ConnectionString = "";
        public static string ConnectionStringForLoggerX = "";
        public static string ConnectionStringForKurumsal = "";
        public static string ConnectionStringForIdari = "";
        public static string DosyaYoluDilekce = "";
        public static string DosyaYoluTarim = "";
        public static string Shared_Dir = "";
        public static string ArsivPath = "";
        public static string MobilPath = "";
        public static string WinLoginUser = "";
        public static string WinLoginPass = "";
        public static string EnvProdOrTest = "";

        public static IWebHostEnvironment? WebHostEnvironment;

        public static void appSettings(IOptions<cConfig> pConfig, IWebHostEnvironment webHostEnvironment)
        {
            cCommon.ConnectionString = cConfig.sConnectionString;
            cCommon.ConnectionStringForLoggerX = cConfig.sConnectionStringForLoggerX;
            cCommon.ConnectionStringForKurumsal = cConfig.sConnectionStringForKurumsal;
            cCommon.ConnectionStringForIdari = cConfig.sConnectionStringForIdari;
            cCommon.DosyaYoluDilekce = cConfig.sDosyaYoluDilekce;
            cCommon.DosyaYoluTarim = cConfig.sDosyaYoluTarim;
            cCommon.WinLoginUser = cConfig.sWinLoginUser;
            cCommon.WinLoginPass = cConfig.sWinLoginPass;
            cCommon.WebHostEnvironment = webHostEnvironment;
            cCommon.Shared_Dir = cConfig.sShared_Dir;
            cCommon.ArsivPath = cConfig.sArsivPath;
            cCommon.MobilPath = cConfig.sMobilPath;
            cCommon.EnvProdOrTest = cConfig.sEnvProdOrTest;
        }

        public static List<string>? DosyaNoValidation(List<string> dosyaNoList)
        {
            try
            {
                List<string> hataliDosyaNumaralari = new List<string>();
                foreach (var item in dosyaNoList)
                {
                    if (item?.Trim().Length != 9)
                    {
                        hataliDosyaNumaralari.Add(item);
                    }
                }

                if (hataliDosyaNumaralari.Count() > 0)
                {
                    return new List<string>() { $"{string.Join(" , ", hataliDosyaNumaralari)} dosya numarası formata uygun değildir." };
                }

                return null;
            }
            catch //(Exception pExc)
            {
                return null;
            }
        }
                
        public static List<cMobileUser>? getMobileUser(int id)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = ConnectionString;

            List<cMobileUser> oMobileUsers = new List<cMobileUser>();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = @" SELECT 
                                    a.ID,
                                    a.Kodu ,
                                    a.AdSoyad ,
                                    a.Email ,
                                    a.UDID ,
                                    mbf.FirmaNo ,
                                    --'tckimlik' tckimlik,
                                    b.Gonderen_TamUnvan FirmaUnvan ,
                                    isnull(RTRIM(b.Adres1),'')  + isnull(RTRIM(b.Adres2),'')  + isnull(RTRIM(b.Adres3),'')  Adres ,
                                    b.Tel ,
                                    b.Fax 
                                FROM [Evrim_DB].[dbo].[MobilKullanici]  a with(nolock)
                                inner join  Evrim_DB.dbo.MobilKullaniciFirma mbf with(nolock) 
                                    on a.ID=mbf.MobilKullaniciID
                                inner join [Evrim_DB].[dbo].[Gonderen] b with(nolock)
                                    on mbf.FirmaNo = b.Gonderen_No 
                                Where 1=1 
                                AND a.ID = " + id.ToString();


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        oMobileUsers = dataTable.AsEnumerable().Select(m => new cMobileUser()
                        {
                            Id = m.Field<int>("Id")
                            ,
                            Kodu = m.Field<string?>("Kodu")
                            ,
                            AdSoyad = m.Field<string?>("AdSoyad")
                            ,
                            Email = m.Field<string?>("Email")
                            ,
                            UDID = m.Field<string?>("UDID")
                            ,
                            FirmaNo = m.Field<int>("FirmaNo")
                            ,
                            Adres = m.Field<string?>("Adres")
                            ,
                            Tel = m.Field<string?>("Tel")
                            ,
                            Fax = m.Field<string?>("Fax")
                        }).ToList();
                    }
                }
                connection.Close();
                return oMobileUsers;
            }
        }

        public static string? getUgmUser(string userCode)
        {
            string ret = "";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = ConnectionString;

            List<cUgmUser> oUgmUsers = new List<cUgmUser>();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = $@" exec UGM_ERP.dbo.pGetUserByCode '{userCode}'";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {
                            ret = reader["Adi_Soyadi"].ToString();
                            break;
                        }
                    }
                    connection.Close();
                    return ret;
                }
            }
        }
        public static List<cUgmUser>? getUgmUser(int id)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = ConnectionString;

            List<cUgmUser> oUgmUsers = new List<cUgmUser>();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = @" select 
                                    Adi_Soyadi AdSoyad,
		                            ISNULL(TCKimlik,'') TcKimlik,
                                    GorevTanimi FirmaUnvan,
                                    Kodu                
                                from Evrim_DB.dbo.users with(nolock)
                                where 1=1 
                                and Users_ID = " + id.ToString();


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        oUgmUsers = dataTable.AsEnumerable().Select(m => new cUgmUser()
                        {
                            Kodu = m.Field<string>("Kodu")
                            ,
                            AdSoyad = m.Field<string?>("AdSoyad")
                            ,
                            TcKimlik = m.Field<string?>("TcKimlik")
                            ,
                            FirmaUnvan = m.Field<string?>("FirmaUnvan")
                        }).ToList();
                    }
                    connection.Close();
                    return oUgmUsers;
                }
            }
        }

        public static List<cBlogImage>? getBlogImages(int pBlogId)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = ConnectionString;

            List<cBlogImage> oBlogImages = new List<cBlogImage>();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = @" select
                                    isnull(link, ''),
                                    isnull(thumbnail_link, ''),
                                    isnull(source, '')
                                from
	                                Evrim_DB.dbo.emobilImage with(nolock)
                                where blogId =  " + pBlogId.ToString();


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cBlogImage oBlogImage = new cBlogImage();

                            oBlogImage.link = reader.GetString(0);
                            oBlogImage.thumbnail_link = reader.GetString(1);
                            string? source = reader.GetString(2);
                            if (!source.IsNullOrEmpty())
                            {
                                if (source != "https://ugm.com.tr/")
                                {
                                    oBlogImage.link = getFotoStringFromMedia(source);
                                }                                                                   
                                else
                                {
                                    oBlogImage.link = source + oBlogImage.link;
                                    oBlogImage.thumbnail_link = source + oBlogImage.thumbnail_link;
                                }                                    
                            } 
                            oBlogImages.Add(oBlogImage);
                        }
                    }
                    connection.Close();
                    return oBlogImages;
                }
            }
        }

        public static User_Info getUserInfoForPost(int? pinternal_user_id, int? pexternal_user_id, bool withPhoto)
        {
            User_Info oUserInfoForPost = new User_Info();
            int internal_user_id = pinternal_user_id == null ? -1 : (int)pinternal_user_id;
            int external_user_id = pexternal_user_id == null ? -1 : (int)pexternal_user_id;

            if (internal_user_id == -1 && external_user_id == -1)
            {
                oUserInfoForPost.internal_user_id = null;
                oUserInfoForPost.external_user_id = null;
                oUserInfoForPost.ad_soyad = "UGM - Bizden Haberler";
                oUserInfoForPost.user_image = ""; //https://ubp.ugm.com.tr:9250/media/images/logo.jpg
                oUserInfoForPost.unvan = "Ünsped Gümrük Müşavirliği ve Lojistik Hizmetler A.Ş.";
                return oUserInfoForPost;
            }

            if (internal_user_id != -1)
            {
                List<cUgmUser> oUgmUsers = getUgmUser(internal_user_id);
                if (oUgmUsers != null && oUgmUsers.Count > 0)
                {
                    oUserInfoForPost.internal_user_id = internal_user_id;
                    oUserInfoForPost.external_user_id = external_user_id;
                    oUserInfoForPost.ad_soyad = oUgmUsers[0].AdSoyad;
                    oUserInfoForPost.user_image = "";
                    if (withPhoto)
                        oUserInfoForPost.user_image = getFotoStringFromTcKimlik(oUgmUsers[0].TcKimlik);
                    
                    oUserInfoForPost.ad_soyad = oUgmUsers[0].AdSoyad;
                }
            }
            else
                if (external_user_id != -1)
                {
                    List<cMobileUser> oMobileUsers = getMobileUser(external_user_id);
                    if (oMobileUsers != null && oMobileUsers.Count > 0)
                    {
                        oUserInfoForPost.internal_user_id = internal_user_id;
                        oUserInfoForPost.external_user_id = external_user_id;
                        oUserInfoForPost.ad_soyad = oMobileUsers[0].AdSoyad;
                        oUserInfoForPost.user_image = ""; //getFotoStringFromTcKimlik(oMobileUsers[0].t);
                        oUserInfoForPost.ad_soyad = oMobileUsers[0].AdSoyad;
                    }
            }

            return oUserInfoForPost;
        }

        public static string? getFotoStringFromTcKimlik(string TcKimlik)
        {
            try
            {
                //UserCredentials credentials = new UserCredentials("UNGROUP", "software", "Sw.2016");
                UserCredentials credentials = new UserCredentials("UNGROUP", WinLoginUser, WinLoginPass);
                using SafeAccessTokenHandle userHandle = credentials.LogonUser(LogonType.Interactive);
#pragma warning disable CA1416 // Validate platform compatibility
                string? base64String = null;
                WindowsIdentity.RunImpersonated(userHandle, () =>
                {
                    byte[] fileContent = null;
                    fileContent = File.ReadAllBytes(Shared_Dir + TcKimlik.Replace(" ", "") + ".jpg");
                    base64String = Convert.ToBase64String(fileContent);                    
                });
                return base64String;
#pragma warning restore CA1416 // Validate platform compatibility                        
            }
            catch (Exception)
            {
                return null;
            }
            //return null;
        }

        public static string? getFotoStringFromMedia(string source)
        {
            try
            {
                //UserCredentials credentials = new UserCredentials("UNGROUP", "software", "Sw.2016");
                UserCredentials credentials = new UserCredentials("UNGROUP", WinLoginUser, WinLoginPass);
                using SafeAccessTokenHandle userHandle = credentials.LogonUser(LogonType.Interactive);
#pragma warning disable CA1416 // Validate platform compatibility
                WindowsIdentity.RunImpersonated(userHandle, () =>
                {
                    byte[] fileContent = null;
                    fileContent = File.ReadAllBytes(Shared_Dir + "media" + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + source.Replace(" ", ""));
                    string? base64String = Convert.ToBase64String(fileContent);
                    return base64String;
                });
#pragma warning restore CA1416 // Validate platform compatibility                        
            }
            catch (Exception)
            {
            }
            return null;
        }

    }
    public class FilingHelper
    {
        public static string FileSave(byte[] bytes, string file_path, string file_name)
        {
            try
            {
                //UserCredentials credentials = new UserCredentials("UNGROUP", "software", "Sw.2016");
                UserCredentials credentials = new UserCredentials("UNGROUP", cCommon.WinLoginUser, cCommon.WinLoginPass);
                using SafeAccessTokenHandle userHandle = credentials.LogonUser(LogonType.Interactive);
#pragma warning disable CA1416 // Validate platform compatibility
                WindowsIdentity.RunImpersonated(userHandle, () =>
                {
                    if (!Directory.Exists(file_path))
                    {
                        Directory.CreateDirectory(file_path);
                    }

                    if (!File.Exists(file_path))
                    {
                        File.WriteAllBytes(file_path + Path.DirectorySeparatorChar + file_name, bytes);
                        //File.Copy(file_path, Directory.GetCurrentDirectory() + file_name);

                    }
                });
#pragma warning restore CA1416 // Validate platform compatibility

                return file_path;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string FileSave_OLD(byte[] bytes, string path, string fileName)
        {
            try
            {
                WindowsLogin wi = new WindowsLogin(cCommon.WinLoginUser, "UNGROUP", cCommon.WinLoginPass);
                //WindowsLogin wi = new WindowsLogin(cCommon.WinLoginUser, "UGMMOBILISS", cCommon.WinLoginPass);
            }
            catch (Exception pExc)
            {
                Exception exc = pExc;
            }
            //string todayStr= DateTime.Now.ToString("yyyyMMdd");
            CreateFolder(path);


            File.WriteAllBytes(path + Path.DirectorySeparatorChar + fileName, bytes);
            return path;
        }
        public static void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static string SaveTarimOdemeFiles(List<string?> images, string pUrunBildirimNo)
        {
            string kayit_path = cCommon.DosyaYoluTarim + pUrunBildirimNo;
            string folder_path = kayit_path + Path.DirectorySeparatorChar + "KayitResimleri";

            //CreateFolder(folder_path);

            foreach (string? image in images)
            {
                string file_ext = ".jpg"; //???
                string file_path = folder_path; // + Path.DirectorySeparatorChar + Guid.NewGuid().ToString() + file_ext;
                byte[] file_content = Convert.FromBase64String(image);

                FileSave(file_content, file_path, Guid.NewGuid().ToString() + file_ext);
            }

            return kayit_path + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyyMMdd");
        }

        public static void WritingException(string pPath, string pText)
        {
            StreamWriter writer = new StreamWriter(pPath, true);
            writer.WriteLine(DateTime.Now.ToString() + " : " + pText);
            writer.Close();
        }
    }

    public class WindowsLogin : IDisposable
    {
        protected const int LOGON32_PROVIDER_DEFAULT = 0;
        protected const int LOGON32_LOGON_INTERACTIVE = 2;

        public WindowsIdentity? Identity;
        private nint m_accessToken;

        [System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain,
        string lpszPassword, int dwLogonType, int dwLogonProvider, ref nint phToken);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private extern static bool CloseHandle(nint handle);


        public WindowsLogin()
        {
#pragma warning disable CA1416 // Validate platform compatibility
            Identity = WindowsIdentity.GetCurrent();
#pragma warning restore CA1416 // Validate platform compatibility
        }

        public WindowsLogin(string username, string domain, string password)
        {
            Login(username, domain, password);
        }

        public void Login(string username, string domain, string password)
        {
            if (Identity != null)
            {
#pragma warning disable CA1416 // Validate platform compatibility
                Identity.Dispose();
#pragma warning restore CA1416 // Validate platform compatibility
                Identity = null;
            }


            try
            {
                m_accessToken = new nint(0);
                Logout();

                m_accessToken = nint.Zero;
                bool logonSuccessfull = LogonUser(
                   username,
                   domain,
                   password,
                   LOGON32_LOGON_INTERACTIVE,
                   LOGON32_PROVIDER_DEFAULT,
                   ref m_accessToken);

                if (!logonSuccessfull)
                {
                    int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                    throw new System.ComponentModel.Win32Exception(error);
                }
#pragma warning disable CA1416 // Validate platform compatibility
                Identity = new WindowsIdentity(m_accessToken);
#pragma warning restore CA1416 // Validate platform compatibility
            }
            catch
            {
                throw;
            }

        } // End Sub Login 

        public void Logout()
        {
            if (m_accessToken != nint.Zero)
                CloseHandle(m_accessToken);

            m_accessToken = nint.Zero;

            if (Identity != null)
            {
#pragma warning disable CA1416 // Validate platform compatibility
                Identity.Dispose();
#pragma warning restore CA1416 // Validate platform compatibility
                Identity = null;
            }

        } // End Sub Logout 


        void IDisposable.Dispose()
        {
            Logout();
        } // End Sub Dispose 


    } // End Class WindowsLogin

    public class LoggerX
    {
        public static async Task LogIn(HttpContext pHttpContext, string pException)
        {
            try
            {
                cLoggerX oLoggerX = new cLoggerX();
                oLoggerX.user_id = 0;
                oLoggerX.user_type = "Internal";
                oLoggerX.request_url = pHttpContext.Request.Path.Value;
                oLoggerX.request_ip = pHttpContext.Connection.RemoteIpAddress?.ToString();
                oLoggerX.query_string = pHttpContext.Request.QueryString.ToString();
                oLoggerX.exception = pException.Replace("'", "");
 
                var bodyStream = new StreamReader(pHttpContext.Request.Body);
                try { 
                    bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                } catch { }
                oLoggerX.request_body = await bodyStream.ReadToEndAsync();

                if (!oLoggerX.request_body.IsNullOrEmpty())
                    if (oLoggerX.request_body.Length > 1000)
                        oLoggerX.request_body = oLoggerX.request_body.Substring(0, 1000);

                foreach (var item in pHttpContext.Request.Query)
                {
                    if (item.Key == "ex_user_id")
                    {
                        oLoggerX.user_type = "External";
                        oLoggerX.user_id = Int32.Parse(item.Value);
                        break;
                    }
                    if (item.Key == "in_user_id")
                    {
                        oLoggerX.user_id = Int32.Parse(item.Value);
                        break;
                    }
                }

                String sql = "";
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = cCommon.ConnectionStringForLoggerX;

                //if (!string.IsNullOrEmpty(oLoggerX.exception))
                //{
                //    pHttpContext.Items.TryGetValue("guid", out var sGuid);

                //    sql = @"UPDATE UGM_Log.dbo.mmu_logs
                //        set exception = '" + oLoggerX.exception + @"'
                //        where 1=1
                //        and guid = '" + sGuid?.ToString() + @"'";
                    
                //    using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                //    {
                //        connection.Open();

                //        SqlCommand command = new SqlCommand(sql, connection);
                
                //        int updatedRow = command.ExecuteNonQuery();
                        
                //        connection.Close();
                //    }
                //}
                //else
                {
                    String sGuid = Guid.NewGuid().ToString();
                    pHttpContext.Items["guid"] = sGuid;
                    sql = @" INSERT INTO UGM_Log.dbo.mmu_logs (request_url, request_ip, exception, request_body, query_string, isProd)
                            VALUES ('" + oLoggerX.request_url + "', '" + oLoggerX.request_ip + "'";
                                        
                    //sql += ", CURRENT_TIMESTAMP";

                    if (string.IsNullOrEmpty(oLoggerX.exception))
                        sql += ", null";
                    else
                        sql += ", '" + oLoggerX.exception + "'";

                    if (string.IsNullOrEmpty(oLoggerX.request_body))
                        sql += ", null";
                    else
                        sql += ", '" + oLoggerX.request_body + "'";

                    if (string.IsNullOrEmpty(oLoggerX.query_string))
                        sql += ", null";
                    else
                        sql += ", '" + oLoggerX.query_string + "'";

                    if (cCommon.EnvProdOrTest == "P")
                        sql += ", 1) ";
                    else
                        sql += ", 0) ";

                    
                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand(sql, connection);
                
                        int insertedRow = command.ExecuteNonQuery();
                        
                        connection.Close();
                    }
                }
            }
            catch { }
        }
    }

    public class MailProcess
    {
        //public static void SendMailBildirim(string From, string To, string CC, string BCC, string Subject, string Body, string DisplayName)
        public static cResponseModel<int> SendMailBildirim(cSendMail pSendMail)
        {
            cResponseModel<int> oResponseModel = new cResponseModel<int>();
            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(pSendMail.from, pSendMail.displayName, Encoding.UTF8);
                msg.To.Add(pSendMail.to);
                if (pSendMail.cc != "") { msg.CC.Add(pSendMail.cc); }
                msg.Subject = pSendMail.subject;
                msg.Body = pSendMail.body;
                msg.IsBodyHtml = true;
                if (pSendMail.bcc != "") { msg.Bcc.Add(pSendMail.bcc); }
                // Dosyayı ekleyin
                //if(file != null)
                //{
                //    foreach (string path in file)
                //    {
                //        Attachment attachment = new Attachment(path, MediaTypeNames.Application.Octet);
                //        msg.Attachments.Add(attachment);
                //    }
                //}

                msg.HeadersEncoding = Encoding.UTF8;
                msg.SubjectEncoding = Encoding.UTF8;
                msg.BodyEncoding = Encoding.UTF8;
 
                using (var smtpClient = new SmtpClient("smtp.office365.com", 587))
                {
                    smtpClient.UseDefaultCredentials = false;
                    //smtpClient.Credentials = new NetworkCredential("bildirim@ugm.com.tr", "Bldr.2021", "domain");
                    if (pSendMail.from == "bildirim@ugm.com.tr" || pSendMail.from == "bildirim1@ugm.com.tr")
                        smtpClient.Credentials = new NetworkCredential(pSendMail.from, "Bldr.2021", "domain");
                    else
                        smtpClient.Credentials = new NetworkCredential(pSendMail.from, pSendMail.password, "domain");
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(msg);
                    smtpClient.Dispose();
                    msg.Dispose();
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("BildirimYgm Mail :  Mail Atarken hata oluştu  :  {0} ,{1}", pSendMail.to, ex.Message);
                oResponseModel.errorModel.ErrorCode = -1;
                oResponseModel.errorModel.ErrorMessage = $"BildirimYgm Mail :  Mail Atarken hata oluştu  :  {pSendMail.to}, {ex.Message}";
            }
            return oResponseModel;
        }
    }
}
