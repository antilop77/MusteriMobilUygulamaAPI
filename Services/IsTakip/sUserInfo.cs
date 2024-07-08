using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Diagnostics;
using MmuAPI.Controllers;
using MmuAPI.Models;
using static System.Net.Mime.MediaTypeNames;
using MusteriMobilUygulamaAPI.Models.IsTakip;
using MmuAPI;
using Microsoft.Win32.SafeHandles;
using SimpleImpersonation;
using System.Security.Principal;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;

namespace MusteriMobilUygulamaAPI.Services.IsTakip
{
    public static class sUserInfo
    {
        //private static ILogger<MmuController>? _logger;

        public static cResponseModel<cUserInfo> get(HttpContext httpContext, ILogger<IsTakipController> logger)
        {
            //int y=0, x = 1/y;
            
            cResponseModel<cUserInfo> oResponseModel = new cResponseModel<cUserInfo>();

            cUserCredential oUserCredential = new cUserCredential();
            httpContext.Items.TryGetValue("oUserCredential", out var x);
            oUserCredential = (cUserCredential)x;

            int? user_id = oUserCredential?.in_user_id;
            int? ex_user_id = oUserCredential?.ex_user_id;
            string? imei = oUserCredential?.imei;


            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = cConfig.sConnectionString;
            cUserInfo oUserInfo = new cUserInfo();
            cDataMmu oDataMmu = new cDataMmu();
            List<cUserAuthorizedForms> list = new List<cUserAuthorizedForms>();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                //def get_user_info(user_id:int):
                string sql = @" select *
                                    , (select 
                                            'all;' + case when departmankod <> '' then (
                                            select 
                                                trim(Topic) 
                                            from 
                                                UGM_ERP.dbo.NotificationTargetGroup N with(nolock) 
                                            where 
                                                N.ID = 3
                                            ) + departmankod + ';' else '' end + case when sektorkod <> '' then (
                                            select 
                                                trim(Topic) 
                                            from 
                                                UGM_ERP.dbo.NotificationTargetGroup N with(nolock) 
                                            where 
                                                N.ID = 1
                                            ) + sektorkod + ';' else '' end + case when bolgekod <> '' then (
                                            select 
                                                trim(Topic) 
                                            from 
                                                UGM_ERP.dbo.NotificationTargetGroup N with(nolock) 
                                            where 
                                                N.ID = 2
                                            ) + bolgekod else '' end
                                        ) NotificationChannels 
                                from 
                                    (
                                    select 
                                        u.Users_Id, 
                                        rtrim(u.Kodu) Kodu, 
                                        trim(isnull(u.Adi_Soyadi,'')) AdiSoyadi,
                                        trim(isnull(u.Mail,'')) email,
                                        STRING_AGG(mf.FormName, ';') FormName, 
                                        trim(
                                        isnull(u.TCKimlik, '')
                                        ) TCKimlik, 
                                        u.Mobil_KuryeNo DusunulenKuryeNo, 
                                        u.DepartmanAdi, 
                                        u.GorevTanimi, 
                                        u.Sektor, 
                                        (
                                        select 
                                            top 1 Tip2 
                                        from 
                                            Evrim_DB.dbo.SektorTip st 
                                        where 
                                            Sektor = Coalesce(
                                            NULLIF(Sektor, ''), 
                                            u.Sektor
                                            )
                                        ) bolge, 
                                        REPLACE(
                                        lower(
                                            isnull(u.Sektor, '')
                                        ), 
                                        ' ', 
                                        ''
                                        ) collate Latin1_General_CI_AS as sektorkod, 
                                        REPLACE(
                                        lower(
                                            isnull(u.DepartmanAdi, '')
                                        ), 
                                        ' ', 
                                        ''
                                        ) collate Latin1_General_CI_AS as departmankod, 
                                        REPLACE(
                                        lower(
                                            isnull(
                                            (
                                                select 
                                                top 1 Tip2 
                                                from 
                                                Evrim_DB.dbo.SektorTip st 
                                                where 
                                                Sektor = Coalesce(
                                                    NULLIF(Sektor, ''), 
                                                    u.Sektor
                                                )
                                            ), 
                                            ''
                                            )
                                        ), 
                                        ' ', 
                                        ''
                                        ) collate Latin1_General_CI_AS as bolgekod 
                                    from 
                                        evrim_db.dbo.users u 
                                        left join UGM_ERP.dbo.UgmModuleRight r on r.Users_Id = u.Users_ID 
                                        and r.Deleted <> 1 
                                        left join UGM_ERP.dbo.UgmUserGroup grp on r.UgmUserGroupId = grp.UgmUserGroupId 
                                        left join UGM_ERP.dbo.UgmModuleForm mf on r.UgmModuleFormId = mf.UgmModuleFormId 
                                        and UgmModuleId = 19 
                                    where 
                                        u.Users_ID = " + oUserCredential?.in_user_id
                                    + @" --and mf.FormName is not null 
                                    group by 
                                        u.Users_Id, 
                                        u.Kodu, 
                                        u.Adi_Soyadi,
                                        u.Mail, 
                                        u.TCKimlik, 
                                        u.Mobil_KuryeNo, 
                                        u.DepartmanAdi, 
                                        u.GorevTanimi, 
                                        u.Sektor
                                    ) as Liste";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            oUserInfo.Users_Id = reader.GetInt32(0);
                            if (!reader[1].Equals(DBNull.Value))
                            {
                                oUserInfo.kodu = reader.GetString(1);
                            }

                            if (!reader[2].Equals(DBNull.Value))
                            {
                                oUserInfo.AdiSoyadi = reader.GetString(2);
                            }

                            if (!reader[3].Equals(DBNull.Value))
                            {
                                oUserInfo.email = reader.GetString(3);
                            }

                            if (!reader[4].Equals(DBNull.Value))
                            {
                                oUserInfo.FormName = reader.GetString(4);
                            }

                            if (!reader[5].Equals(DBNull.Value))
                            {
                                oUserInfo.TCKimlik = reader.GetString(5);
                            }

                            if (!reader[6].Equals(DBNull.Value))
                            {
                                oUserInfo.DusunulenKuryeNo = reader.GetInt16(6);
                            }

                            if (!reader[7].Equals(DBNull.Value))
                            {
                                oUserInfo.DepartmanAdi = reader.GetString(7);
                            }

                            if (!reader[8].Equals(DBNull.Value))
                            {
                                oUserInfo.GorevTanimi = reader.GetString(8);
                            }

                            if (!reader[9].Equals(DBNull.Value))
                            {
                                oUserInfo.Sektor = reader.GetString(9);
                            }

                            if (!reader[10].Equals(DBNull.Value))
                            {
                                oUserInfo.bolge = reader.GetString(10);
                            }

                            if (!reader[11].Equals(DBNull.Value))
                            {
                                oUserInfo.sektorkod = reader.GetString(11);
                            }

                            if (!reader[12].Equals(DBNull.Value))
                            {
                                oUserInfo.departmankod = reader.GetString(12);
                            }

                            if (!reader[13].Equals(DBNull.Value))
                            {
                                oUserInfo.bolgekod = reader.GetString(13);
                            }

                            if (!reader[14].Equals(DBNull.Value))
                            {
                                oUserInfo.NotificationChannels = reader.GetString(14);
                            }

                            break;
                        }
                    }
                }

                //def get_mmu_user_code(user_id):
                sql = @"SELECT TRIM(ISNULL(Kodu,'')) Kodu
                        FROM [Evrim_DB].[dbo].[MobilKullanici] with(nolock)
                        Where ID = " + oUserCredential?.ex_user_id;
                string mmu_user_code = "";
                try
                {

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!reader[0].Equals(DBNull.Value))
                                {
                                    mmu_user_code = reader.GetString(0);
                                }

                                break;
                            }
                        }
                    }
                }
                catch { }

                //data_mmu = get_user_info_mmu(mmu_user_code,imei)
                List<cFirmalar> oFirmalar = new List<cFirmalar>();
                List<cKullaniciBilgileri> oKullaniciBilgileri = new List<cKullaniciBilgileri>();

                sql = @"SELECT a.ID,
                            a.Kodu ,
                            a.AdSoyad ,
                            a.Email ,
                            a.UDID ,
                            mbf.FirmaNo ,
                            'tckimlik' tckimlik,
                            b.Gonderen_TamUnvan FirmaUnvan ,
                            isnull(RTRIM(b.Adres1),'')  + isnull(RTRIM(b.Adres2),'')  + isnull(RTRIM(b.Adres3),'')  adres ,
                            b.Tel ,
                            b.Fax 
                            , hct.Kullanici
							, TRIM(u.Kodu) hct_Kodu,
				            TRIM(ISNULL(u.Adi_Soyadi, '')) Adi_Soyadi,
				            TRIM(ISNULL(u.Mail, '')) Mail,
				            TRIM(ISNULL(u.Unvani, '')) Unvani ,
				            TRIM(ISNULL(u.GorevTanimi, '')) GorevTanimi,
				            TRIM(ISNULL(u.TCKimlik , '')) TcKimlik2,
				            TRIM(ISNULL(u.BolumSorumlusu , '')) BolumSorumlusu
                        FROM [Evrim_DB].[dbo].[MobilKullanici]  a with(nolock)
                        inner join  Evrim_DB.dbo.MobilKullaniciFirma mbf with(nolock) 
                            on a.ID=mbf.MobilKullaniciID
                        inner join [Evrim_DB].[dbo].[Gonderen] b with(nolock)
                            on mbf.FirmaNo = b.Gonderen_No 
                        left join [Evrim_DB].[dbo].[HspCarTem] hct with(nolock) 
							on hct.Gonderen_No = mbf.FirmaNo --'{firma_no}'
						left join [Evrim_DB].[dbo].[Users] u with(nolock)
							on u.Kodu = hct.Kullanici
                        Where 1=1
                        and ISNULL(u.Iptal,'')!= 'X'
                        and a.Kodu = '" + mmu_user_code + @"' 
                        and a.UDID = '" + oUserCredential?.imei + "'";
                //logger.LogInformation(sql);

                try
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                oDataMmu.ID = reader.GetInt32(0);
                                if (!reader[1].Equals(DBNull.Value))
                                {
                                    oDataMmu.Kodu = reader.GetString(1);
                                }

                                if (!reader[2].Equals(DBNull.Value))
                                {
                                    oDataMmu.AdSoyad = reader.GetString(2);
                                }

                                if (!reader[3].Equals(DBNull.Value))
                                {
                                    oDataMmu.Email = reader.GetString(3);
                                }

                                if (!reader[4].Equals(DBNull.Value))
                                {
                                    oDataMmu.UDID = reader.GetString(4);
                                }

                                oDataMmu.FirmaNo = reader.GetInt32(5);
                                if (!reader[6].Equals(DBNull.Value))
                                {
                                    oDataMmu.tckimlik = reader.GetString(6);
                                }

                                if (!reader[7].Equals(DBNull.Value))
                                {
                                    oDataMmu.FirmaUnvan = reader.GetString(7);
                                }

                                if (!reader[8].Equals(DBNull.Value))
                                {
                                    oDataMmu.adres = reader.GetString(8);
                                }

                                if (!reader[9].Equals(DBNull.Value))
                                {
                                    oDataMmu.Tel = reader.GetString(9);
                                }

                                if (!reader[10].Equals(DBNull.Value))
                                {
                                    oDataMmu.Fax = reader.GetString(10);
                                }

                                if (!reader[11].Equals(DBNull.Value))
                                {
                                    oDataMmu.hct_Kodu = reader.GetString(11);
                                }

                                if (!reader[12].Equals(DBNull.Value))
                                {
                                    oDataMmu.Adi_Soyadi = reader.GetString(12);
                                }

                                if (!reader[13].Equals(DBNull.Value))
                                {
                                    oDataMmu.Adi_Soyadi = reader.GetString(13);
                                }

                                if (!reader[14].Equals(DBNull.Value))
                                {
                                    oDataMmu.Mail = reader.GetString(14);
                                }

                                if (!reader[15].Equals(DBNull.Value))
                                {
                                    oDataMmu.Unvani = reader.GetString(15);
                                }

                                if (!reader[16].Equals(DBNull.Value))
                                {
                                    oDataMmu.GorevTanimi = reader.GetString(16);
                                }

                                if (!reader[17].Equals(DBNull.Value))
                                {
                                    oDataMmu.TcKimlik2 = reader.GetString(17);
                                }

                                if (!reader[18].Equals(DBNull.Value))
                                {
                                    oDataMmu.BolumSorumlusu = reader.GetString(18);
                                }

                                cFirmalar oFirma = new cFirmalar();

                                oFirma.FirmaNo = oDataMmu.FirmaNo;
                                oFirma.FirmaUnvan = oDataMmu.FirmaUnvan;
                                oFirma.adres1 = oDataMmu.adres;
                                oFirma.Tel = oDataMmu.Tel;
                                oFirma.Fax = oDataMmu.Fax;

                                oFirmalar.Add(oFirma);

                                cKullaniciBilgileri oKullaniciBilgi = new cKullaniciBilgileri();
                                oKullaniciBilgi.Kodu = oDataMmu.hct_Kodu;
                                oKullaniciBilgi.Adi_Soyadi = oDataMmu.Adi_Soyadi;
                                oKullaniciBilgi.Mail = oDataMmu.Mail;
                                oKullaniciBilgi.Unvani = oDataMmu.Unvani;
                                oKullaniciBilgi.GorevTanimi = oDataMmu.GorevTanimi;
                                oKullaniciBilgi.TcKimlik = oDataMmu.TcKimlik2;
                                oKullaniciBilgi.BolumSorumlusu = oDataMmu.BolumSorumlusu;
                                //string mediaPath = "";
                                //try
                                //{
                                //    using (WindowsLogin wi = new WindowsLogin(cCommon.WinLoginUser, Environment.MachineName, cCommon.WinLoginPass))
                                //    {
                                //        mediaPath = cCommon.MobilPath + "media";
                                //        File.Copy(cCommon.Shared_Dir + oKullaniciBilgi.TcKimlik + ".jpg", mediaPath + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + oKullaniciBilgi.TcKimlik + ".jpg", true);
                                //    }

                                //}
                                //catch (Exception pExc)
                                //{
                                //    Exception exc = pExc;
                                //    FilingHelper.WritingException(Path.Combine(cCommon.WebHostEnvironment?.ContentRootPath, "media") + Path.DirectorySeparatorChar + "Exception.log", pExc.Message);
                                //}

                                //oKullaniciBilgi.resim = httpContext.Request.Scheme + "://" + httpContext.Request.Host + "/media/images/" + oKullaniciBilgi.TcKimlik + ".jpg";
                                if (!oKullaniciBilgi.TcKimlik.IsNullOrEmpty())
                                {   
                                    try
                                    {
                                        //UserCredentials credentials = new UserCredentials("UNGROUP", "software", "Sw.2016");
                                        UserCredentials credentials = new UserCredentials("UNGROUP", cCommon.WinLoginUser, cCommon.WinLoginPass);
                                        using SafeAccessTokenHandle userHandle = credentials.LogonUser(LogonType.Interactive);
#pragma warning disable CA1416 // Validate platform compatibility
                                        WindowsIdentity.RunImpersonated(userHandle, () =>
                                        {
                                            byte[] fileContent = null;
                                            fileContent = File.ReadAllBytes(cCommon.Shared_Dir + oKullaniciBilgi.TcKimlik + ".jpg");
                                            string? base64String = Convert.ToBase64String(fileContent);
                                            oKullaniciBilgi.resim = base64String;
                                        });
#pragma warning restore CA1416 // Validate platform compatibility                        
                                    }
                                    catch (Exception)
                                    {                                        
                                    }
                                }

                                oKullaniciBilgileri.Add(oKullaniciBilgi);
                            }
                            if (oUserInfo.email == "")
                            {
                                oUserInfo.email = oDataMmu.Email;
                            }

                            oUserInfo.firmalar = oFirmalar;
                            oUserInfo.message_users = oKullaniciBilgileri;
                        }
                    }
                }
                catch { }

                //def get_forms():                    
                sql = @"select FormName from UGM_ERP.dbo.UgmModuleForm umf where UgmModuleId = 19";

                try
                {

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!reader[0].Equals(DBNull.Value))
                                {
                                    string FormName = reader.GetString(0);
                                    cUserAuthorizedForms oUserAuthorizedForms = new cUserAuthorizedForms();
                                    oUserAuthorizedForms.formId = FormName;
                                    oUserAuthorizedForms.formAccess = false;
                                    string? formName = oUserInfo.FormName;
                                    if (!formName.IsNullOrEmpty())
                                    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                                        if (formName.Contains(FormName))
                                            oUserAuthorizedForms.formAccess = true;                                            
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                                    }
                                    list.Add(oUserAuthorizedForms);
                                }
                            }
                            oUserInfo.UserAuthorizedForms = list;
                        }
                    }
                }
                catch { }

                connection.Close();
            }
            //oUserInfo.TCKimlik = "16052863876.jpg";
            if (user_id == 5348) // MOBILTEST
            {
                oUserInfo.TCKimlik = "UnspedLogo";
            }

            if (!oUserInfo.TCKimlik.IsNullOrEmpty())
            {
                try
                {
                    //UserCredentials credentials = new UserCredentials("UNGROUP", "software", "Sw.2016");
                    UserCredentials credentials = new UserCredentials("UNGROUP", cCommon.WinLoginUser, cCommon.WinLoginPass);
                    using SafeAccessTokenHandle userHandle = credentials.LogonUser(LogonType.Interactive);
#pragma warning disable CA1416 // Validate platform compatibility
                    WindowsIdentity.RunImpersonated(userHandle, () =>
                    {
                        byte[] fileContent = null;
                        fileContent = File.ReadAllBytes(cCommon.Shared_Dir + oUserInfo.TCKimlik + ".jpg");
                        string? base64String = Convert.ToBase64String(fileContent);
                        oUserInfo.user_image = base64String;
                    });
#pragma warning restore CA1416 // Validate platform compatibility                        
                }
                catch (Exception)
                {
                    throw;
                }
            }
            List<cUserInfo> items = new List<cUserInfo>();
            items.Add(oUserInfo);
            oResponseModel.Data = items;
            return oResponseModel; 
        }
    }
}
