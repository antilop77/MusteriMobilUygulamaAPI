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

namespace MusteriMobilUygulamaAPI.Controllers
{
    [Route("para-talep")]
    [ApiController]
    [CheckToken]
    public class ParaTalepController : ControllerBase
    {
        private readonly ILogger<ParaTalepController> _logger;
        private cConfig oConfig = new cConfig();

        public ParaTalepController(ILogger<ParaTalepController> logger, IOptions<cConfig> pConfig, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            oConfig = pConfig.Value;
            cCommon.appSettings(pConfig, webHostEnvironment);
        }

        [HttpGet("parataleponaylistesi")]
        public IActionResult ParaTalepOnayListesi([FromQuery] string? baslangicTarihi, string? bitisTarihi)
        {
            return Ok(sParaTalepOnayListesi.getParaTalepOnayListesi(DateTime.Parse(baslangicTarihi), DateTime.Parse(bitisTarihi)));
        }

        [HttpGet("parataleponaylama")]
        public IActionResult getParaTalepOnaylama([FromQuery] int ID, string onayTarih, string onaySaati, int detayNo)
        {
            return Ok(sParaTalep.getParaTalepOnaylama(ID, DateTime.Parse(onayTarih), onaySaati, detayNo, ((cUserCredential)HttpContext.Items["oUserCredential"])?.in_user_id));
        }

        public class PrmGuncelleme
        {
            public int ID { get; set;} = -1;
            public decimal Tutar { get; set;} = -1;            
        }

        [HttpPost("paratalepguncelleme")]
        public IActionResult postParaTalepGuncelleme(PrmGuncelleme prmGuncelleme)
        {
            return Ok(sParaTalep.postParaTalepGuncelleme(prmGuncelleme.ID, prmGuncelleme.Tutar, ((cUserCredential)HttpContext.Items["oUserCredential"])?.in_user_id));
        }

        [HttpPost("paratalepsilme")]
        public IActionResult PostParaTalepSilme(PrmGuncelleme prm)
        {
            return Ok(sParaTalep.postParaTalepSilme(prm.ID));
        }

        [HttpGet("ceza-parataleponay/")]
        public IActionResult CezaParaTalepOnay()
        {
            return Ok(sParaTalepCezaOnay.getParaTalepCezaOnay());
        }

        [HttpPost("ceza-paratalepkaydet/")]
        public IActionResult CezaParaTalepKaydet(string onay_red, int detay_no, string isi_veren, string kullanici_kodu, string onay_not)
        {
            return Ok(sParaTalepCezaKaydet.postParaTalepCezaKaydet(detay_no, onay_red, isi_veren, kullanici_kodu, onay_not));
        }

        [HttpGet("odeme-parataleponay/")]
        public IActionResult OdemeParaTalepOnay()
        {
            return Ok(sParaTalepOdemeOnay.getParaTalepOdemeOnay());
        }

        [HttpPost("odeme-paratalepkaydet/")]
        public IActionResult OdemeParaTalepKaydet(string onay_red, int detay_no, string isi_veren, string kullanici_kodu, string onay_not)
        {
            return Ok(sParaTalepOdemeKaydet.postParaTalepOdemeKaydet(detay_no, onay_red, isi_veren, kullanici_kodu, onay_not));
        }
    }
}
