using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MmuAPI;
using MmuAPI.Controllers;
using MmuAPI.Models;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Models;
using MusteriMobilUygulamaAPI.Models.Api;
using MusteriMobilUygulamaAPI.Models.IsTakip;
using MusteriMobilUygulamaAPI.Services.Api;
using MusteriMobilUygulamaAPI.Services.IsTakip;

namespace MusteriMobilUygulamaAPI.Controllers
{
    [ApiController]
    [Route("api")]
    [CheckToken] 
    public class ApiController : ControllerBase
    {
        private readonly ILogger<IsTakipController> _logger;
        private cConfig oConfig = new cConfig();
        public ApiController(ILogger<IsTakipController> logger, IOptions<cConfig> pConfig, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            oConfig = pConfig.Value;
            cCommon.appSettings(pConfig, webHostEnvironment);
        }
        

        [HttpGet("emobilBlog")]
        public IActionResult EmobilBlog([FromQuery] string page_number, string count)
        {
            _ = LoggerX.LogIn(HttpContext, "");
            return Ok(sEmobilBlog.getEmobilBlog(Int32.Parse(page_number), Int32.Parse(count)));
        }

        [HttpGet("emobilBlogLikes")]
        public IActionResult EmobilBlogLikes([FromQuery] string blogId)
        {
            _ = LoggerX.LogIn(HttpContext, "");
            return Ok(sEmobilBlogLikes.getEmobilBlogLikes(Int32.Parse(blogId), true));
        }

        [HttpPost("blog-like")]
        public IActionResult BlogLike(cEmobilBlogReq pEmobilBlogReq)
        {
            cUserCredential oUserCredential = (cUserCredential) HttpContext.Items["oUserCredential"];
            return Ok(sEmobilBlogLike.postEmobilBlogLike(pEmobilBlogReq.blog_id, oUserCredential?.in_user_id, oUserCredential?.ex_user_id));
        }

        [HttpGet("savunma-girilmesi-gereken-kodlar")]
        public IActionResult SavunmaGirilmesiGerekenKodlar()
        {
            cUserCredential oUserCredential = (cUserCredential) HttpContext.Items["oUserCredential"];
            return Ok(sSavunmaGirilmesiGerekenKodlar.getSavunmaGirilmesiGerekenKodlar(oUserCredential));
        }

        [HttpPost("savunma-girilmesi-gereken-kodlar")]
        public IActionResult postSavunmaGirilmesiGerekenKodlar([FromQuery] string is_takipt_id, string kulnot)
        {
            return Ok(sSavunmaGirilmesiGerekenKodlar.postSavunmaGirilmesiGerekenKodlar(is_takipt_id, kulnot));
        }

        [HttpGet("kys-belgeleri")]
        public IActionResult getKysBelgeleri()
        {
            return Ok(sKysBelgeleri.getKysBelgeleri());
        }

        [HttpGet("Dil")]
        public IActionResult getDil()
        {
            return Ok(sBilgilendirme.getDil());
        }

        [HttpGet("emobilSoruCevap")]
        public IActionResult getEmobilSoruCevap([FromQuery] string Tip, string Dil)
        {
            return Ok(sBilgilendirme.getEmobilSoruCevap(Tip, Dil));
        }

        [HttpGet("emobilSoruCevapTip")]
        public IActionResult getEmobilSoruCevapTip([FromQuery] string Dil)
        {
            return Ok(sBilgilendirme.getEmobilSoruCevapTip(Dil));
        }

        [HttpGet("emobilOfis")]
        public IActionResult getEmobilOfis()
        {
            return Ok(sIletisim.getEmobilOfis());
        }

        [HttpPost("send-mail")]
        public IActionResult postSendMail(cSendMail oSendMail)
        {
            return Ok(MailProcess.SendMailBildirim(oSendMail));
        }

        [HttpGet("KullaniciKaydi")]
        public IActionResult getKullaniciKaydi([FromQuery] string tc_no, string evrim_kodu, string ad_soyad, string email, string imei)
        {
            return Ok(sKullaniciKaydi.getKullaniciKaydi(HttpContext, tc_no, evrim_kodu, ad_soyad, email, imei));
        }

        [HttpGet("kullanici-sil")]
        public IActionResult getKullaniciSil([FromQuery] string userID)
        {
            return Ok(sKullaniciKaydi.getKullaniciSil(userID));
        }

        [HttpGet("MailAuth")]
        public IActionResult getMailAuth([FromQuery] string evrim_kodu, string auth_key)
        {
            return Ok(sKullaniciKaydi.getMailAuth(evrim_kodu, auth_key));
        }        

        [HttpGet("BekleyenIsler")]
        public IActionResult getBekleyenIsler([FromQuery] string firma_no, string IslemTipi, string Tip, string kullanici, string parola, string udid)
        {
            return Ok(sBekleyenIsler.getBekleyenIsler(firma_no, IslemTipi, Tip, kullanici, parola, udid));
        }

        [HttpGet("emobilIstakipSorgulama")]
        public IActionResult getEmobilIstakipSorgulama([FromQuery] string firma_no, string referans, string referans_tipi, string tip, string kullanici, string parola)
        {
            return Ok(sEmobilIstakipSorgulama.getEmobilIstakipSorgulama(HttpContext, firma_no, referans, referans_tipi, tip, kullanici, parola));
        }

        [HttpGet("emobilIthalatRaporu")]
        public IActionResult getEmobilIthalatRaporu([FromQuery] string firma_no, string tescil_tarihi_1, string tescil_tarihi_2, string kullanici, string parola, string udid)
        {
            return Ok(sEmobilIhracatIthalatRaporu.getEmobilIthalatRaporu(firma_no, tescil_tarihi_1, tescil_tarihi_2, kullanici, parola, udid));
        }

        [HttpGet("IhracatRaporu")]
        public IActionResult getIhracatRaporu([FromQuery] string firma_no, string tescil_tarihi_1, string tescil_tarihi_2, string kullanici, string parola, string udid)
        {
            return Ok(sEmobilIhracatIthalatRaporu.getIhracatRaporu(firma_no, tescil_tarihi_1, tescil_tarihi_2, kullanici, parola, udid));
        }

        [HttpGet("FaturaSorgu")]
        public IActionResult getFaturaSorgu([FromQuery] string FirmaNo, string Kullanici, string Parola, string IlkTarih, string SonTarih, string UDID)
        {
            return Ok(sServisler.getFaturaSorgu(FirmaNo, Kullanici, Parola, IlkTarih, SonTarih, UDID));
        }

        [HttpGet("FaturaNoSorgu")]
        public IActionResult getFaturaNoSorgu([FromQuery] string FirmaNo, string Kullanici, string Parola, string Tip, string FaturaNo, string Yil, string UDID)
        {
            return Ok(sServisler.getFaturaNoSorgu(FirmaNo, Kullanici, Parola, Tip, FaturaNo, Yil, UDID));
        }

        [HttpGet("MobilLogin2")]
        public IActionResult getMobilLogin2([FromQuery] string firma_no, string kullanici, string parola, string udid)
        {
            return Ok(sServisler.getMobilLogin2(firma_no, kullanici, parola, udid));
        }

        [HttpGet("BakiyeBorc")]
        public IActionResult getBakiyeBorc([FromQuery] string firma_no, string kullanici, string parola, string udid)
        {
            return Ok(sServisler.getBakiyeBorc(firma_no, kullanici, parola, udid));
        }

        [HttpGet("emobilIslemAdet")]
        public IActionResult getEmobilIslemAdet([FromQuery] string firma_no, string tip, string kullanici, string parola, string tarih1, string tarih2, string udid)
        {
            return Ok(sServisler.getEmobilIslemAdet(firma_no, tip, kullanici, parola, tarih1, tarih2, udid));
        }

        [HttpGet("BeyannameDetay")]
        public IActionResult getBeyannameDetay([FromQuery] string firma_no, string tip, string dosyano)
        {
            return Ok(sServisler.getBeyannameDetay(firma_no, tip, dosyano));
        }

        [HttpGet("BeyannameDetayKalem")]
        public IActionResult getBeyannameDetayKalem([FromQuery] string firma_no, string tip, string dosyano)
        {
            return Ok(sServisler.getBeyannameDetayKalem(firma_no, tip, dosyano));
        }

        [HttpGet("BeyannameMasraf")]
        public IActionResult getBeyannameMasraf([FromQuery] string firma_no, string tip, string dosyano)
        {
            return Ok(sServisler.getBeyannameMasraf(firma_no, tip, dosyano));
        }
    }
}
