using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using MusteriMobilUygulamaAPI.Models.IsTakip;
using MusteriMobilUygulamaAPI.Services.IsTakip;
using MusteriMobilUygulamaAPI.Common;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;


namespace MmuAPI.Controllers
{
    [ApiController]
    [Route("is-takip")]
    [CheckToken]
    public class IsTakipController : ControllerBase
    {
        private readonly ILogger<IsTakipController> _logger;
        private cConfig oConfig = new cConfig();
        public IsTakipController(ILogger<IsTakipController> logger, IOptions<cConfig> pConfig, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            oConfig = pConfig.Value;
            cCommon.appSettings(pConfig, webHostEnvironment);
        }      

        [HttpGet("son-islemler")]
        public IActionResult SonIslemler([FromQuery] string kod)
        {
            return Ok(sSonIslemler.get(kod));
        }

        [HttpGet("gecikme-sebebi")]
        public IActionResult GecikmeSebebi()
        {
            return Ok(sGecikmeSebebi.get());
        }

        [HttpPost("dosya-bilgi")]
        public IActionResult DosyaBilgi([FromQuery] string kod, string imei, string belge_no)
        {
            return Ok(sDosyaBilgi.get(kod, imei, belge_no));
        }

        [HttpPost("secimli")]
        public IActionResult Secimli([FromQuery] string kod, string imei, string tip, string gumruk, string is_takip_kod)
        {
            return Ok(sSecimli.get(kod, imei, tip, gumruk, is_takip_kod));
        }

        [HttpGet("gosterge")]
        public IActionResult Gosterge()
        {
            return Ok(sGosterge.get());
        }

        [HttpGet("kod-tarih")]
        public IActionResult KodTarih([FromQuery] string dosya_no, string dosya_tip)
        {
            return Ok(sKodTarih.get(dosya_no, dosya_tip));
        }

        [HttpGet("tip")]
        public IActionResult Tip()
        {
            return Ok(sTip.get());
        }

        [HttpGet("gumrukler")]
        public IActionResult Gumrukler()
        {
            return Ok(sGumrukler.get());
        }

        [HttpGet("istakip-kod")]
        public IActionResult IstakipKod()
        {
            return Ok(sIstakipKod.get());
        }

        [HttpGet("mobil-fotograf-tip")]
        public IActionResult MobilFotografTip()
        {
            return Ok(sMobilFotografTip.get());
        }

        [HttpPost("arsive-yukle2")]
        public IActionResult ArsiveYukle2(cArsiveYukle2 param)
        {
            return Ok(sArsiveYukle2.post(param));
        }

        [HttpPost("dosya-tam-urun-bildirimleri")]
        public IActionResult DosyaTamUrunBildirimleri([FromQuery] String baslangic_tarihi, String bitis_tarihi)
        {
            CultureInfo tr = new CultureInfo("tr-TR");
            string format = "yyyy'-'MM'-'dd";

            return Ok(sDosyaTamUrunBildirimleri.get(DateTime.ParseExact(baslangic_tarihi, format, tr), DateTime.ParseExact(bitis_tarihi, format, tr)));
        }

        [HttpGet("tarim-odeme-tipleri")]
        public IActionResult TarimOdemeTipleri()
        {
            return Ok(sTarimOdemeTipleri.get());
        }

        [HttpGet("mobil-app-version")]
        public IActionResult MobilAppVersion(String application_name)
        {
            return Ok(sMobilAppVersion.get(application_name));
        }

        [HttpGet("kullanici-dosyalari")]
        public IActionResult KullaniciDosyalari(String kullanici)
        {
            return Ok(sKullaniciDosyalari.get(kullanici));
        }

        //Emre Gemici
        [HttpGet("dosya-no-kontrol")]
        public IActionResult DosyaNoKontrol([FromQuery] cDosyaNoKontrolInput param)
        {
            return Ok(sDosyaNoKontrol.get(param));
        }

        //Emre Gemici
        [HttpGet("dilekce-no-getir")]
        public IActionResult DilekceNoGetir([FromQuery] cDilekceNoGetirInput param)
        {
            return Ok(sDilekceNoGetir.get(param));
        }

        //Emre Gemici
        [HttpPost("dilekce-ek-kayit")]
        public IActionResult DilekceEkKayit(cDilekceEkKayit param)
        {
            return Ok(sDilekceEkKayit.get(param));
        }

        [HttpPost("tarim-odeme-kayit-gorseli-yukle")]
        public IActionResult TarimOdemeKayitGorseliYukle(cTarimOdemeKayitGorseliYukle param)
        {
            return Ok(sTarimOdemeKayitGorseliYukle.post(param));
        }

        [HttpGet("user-info")]
        public IActionResult UserInfo()
        {   
            return Ok(sUserInfo.get(HttpContext, _logger));
        }

        [HttpPost("is-takip-new-2")]
        public IActionResult IsTakipNewIki(cIsTakipNew2 param)
        {
            return Ok(sIsTakipNew2.IsTakipNewIki(param));
        }
    }
}
