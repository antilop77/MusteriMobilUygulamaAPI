using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MmuAPI.Controllers;
using MmuAPI;
using Microsoft.Extensions.Options;
using MusteriMobilUygulamaAPI.Common;
using MusteriMobilUygulamaAPI.Services.Api;
using MusteriMobilUygulamaAPI.Services.ParaTalep;
using MusteriMobilUygulamaAPI.Models.IsTakip;
using System.Text.Json.Serialization;
using MmuAPI.Models;
using System;
using MusteriMobilUygulamaAPI.Services.Intranet;
using static System.Net.Mime.MediaTypeNames;
using System.Globalization;

namespace MusteriMobilUygulamaAPI.Controllers
{
    [Route("intranet")]
    [ApiController]
    [CheckToken]
    public class IntranetController : ControllerBase
    {
        private readonly ILogger<IntranetController> _logger;
        private cConfig oConfig = new cConfig();

        public IntranetController(ILogger<IntranetController> logger, IOptions<cConfig> pConfig, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            oConfig = pConfig.Value;
            cCommon.appSettings(pConfig, webHostEnvironment);
        }

        [HttpGet("tanitim-video")]
        public IActionResult getTanitimVideo()
        {
            return Ok(sTanitimVideo.getTanitimVideo());
        }

        [HttpGet("katalog")]
        public IActionResult getKatalog()
        {
            return Ok(sKatalog.getKatalog());
        }

        [HttpGet("kurumsal-derecelendirme")]
        public IActionResult getKurumsalDerecelendirme()
        {
            return Ok(sKurumsalDerecelendirme.getKurumsalDerecelendirme());
        }

        [HttpGet("mali-denetim")]
        public IActionResult getMaliDenetim()
        {
            return Ok(sMaliDenetim.getMaliDenetim());
        }

        [HttpGet("oda-dernek")]
        public IActionResult getOdaDernek()
        {
            return Ok(sOdaDernek.getOdaDernek());
        }

        [HttpGet("etik-kurallar")]
        public IActionResult getEtikKurallar()
        {
            return Ok(sEtikKurallar.getEtikKurallar());
        }
        
        [HttpGet("get-kurumsalisbirliklerimiz")]
        public IActionResult getKurumsalIsbirliklerimiz()
        {
            return Ok(sKurumsalIsbirliklerimiz.getKurumsalIsbirliklerimiz());
        }

        [HttpGet("get-lokasyonlarimiz")]
        public IActionResult getLokasyonlarimiz()
        {
            return Ok(sLokasyonlarimiz.getLokasyonlarimiz());
        }

        [HttpGet("get-personelservisguzergah")]
        public IActionResult getPersonelServisGuzergah()
        {
            return Ok(sPersonelServisGuzergah.getPersonelServisGuzergah());
        }
        
        [HttpGet("get-kuryeringsaatleri")]
        public IActionResult getKuryeRingSaatleri()
        {
            return Ok(sKuryeRingSaatleri.getKuryeRingSaatleri());
        }

        [HttpGet("haftalik-yemek")]
        public IActionResult getHaftalikYemek()
        {
            return Ok(sHaftalikYemek.getHaftalikYemek());
        }

        [HttpPost("bir-sorum-var")]
        public IActionResult postBirSorumVar([FromQuery] string konu, string soru, string username)
        {
            return Ok(sBirSorumVar.postBirSorumVar(konu, soru, username));
        }

        [HttpPost("bir-fikrim-var")]
        public IActionResult postBirFikrimVar([FromQuery] string konu, string fikir, string username)
        {
            return Ok(sBirFikrimVar.postBirFikrimVar(konu, fikir, username));
        }
        

        [HttpGet("yeni-yasin-kutlu-olsun")]
        public IActionResult getYeniYasinKutluOlsun()
        {
            return Ok(sYeniYasinKutluOlsun.getYeniYasinKutluOlsun());
        }
        
        [HttpGet("kan-ihtiyaci")]
        public IActionResult getKanIhtiyaci()
        {
            return Ok(sKanIhtiyaci.getKanIhtiyaci());
        }

        [HttpGet("izin-duyurulari")]
        public IActionResult getIzinDuyurulari()
        {
            return Ok(sIzinDuyurulari.getIzinDuyurulari());
        }

        [HttpGet("get-rehberdepartmanlistesi")]
        public IActionResult getRehberDepartmanListesi()
        {
            return Ok(sIntranet.getRehberDepartmanListesi());
        }

        [HttpGet("get-rehbermusavirkarnelistesi")]
        public IActionResult getRehberMusavirKarneListesi()
        {
            return Ok(sIntranet.getRehberMusavirKarneListesi());
        }

        [HttpGet("get-rehbergorevlistesi")]
        public IActionResult getRehberGorevListesi()
        {
            return Ok(sIntranet.getRehberGorevListesi());
        }

        [HttpGet("get-rehbersorgulama")]
        public IActionResult getRehberSorgulama([FromQuery] string? adsoyad, string? gorev, string? departman, string? musavirkarne, string? dahili, string? kisaKod, string? BolgeTanimi, string? SubeTanimi)
        {
            if (musavirkarne is null)
                return Ok(sIntranet.getRehberSorgulama(adsoyad, gorev, departman, dahili, kisaKod, BolgeTanimi, SubeTanimi));
            else
                return Ok(sIntranet.getMusavirKarneListesi(adsoyad, gorev, departman, dahili, kisaKod, BolgeTanimi, SubeTanimi));
        }

        [HttpGet("getBolgeler")]
        public IActionResult getBolgeler()
        {
            return Ok(sIntranet.getBolgeler());
        }

        [HttpGet("getSubeler")]
        public IActionResult getSubeler()
        {
            return Ok(sIntranet.getSubeler());
        }
    }
}
