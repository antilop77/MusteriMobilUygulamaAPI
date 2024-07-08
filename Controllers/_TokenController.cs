using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using MmuAPI.Models;
using RestSharp;
using MusteriMobilUygulamaAPI.Services;


namespace MmuAPI.Controllers
{
    [Route("login")]
    [ApiController]
    public class _TokenController : ControllerBase
    {
        private readonly IJwtTokenManager iJwtTokenManager;
        public _TokenController(IJwtTokenManager pIJwtTokenManager)
        {
            iJwtTokenManager = pIJwtTokenManager;
        }


        [AllowAnonymous]
        [Consumes("multipart/form-data")]
        [HttpPost("token")]
        public IActionResult postToken([FromQuery] cUser pUser)
        {
            //if (pUser.username != "METIND")
            {
                string ret = sLogin.checkUserPassword(pUser, HttpContext);
                if (ret != "")
                    return Ok(ret);
            }

            cUserCredential oUserCredential;// = new cUserCredential();
            HttpContext.Items.TryGetValue("oUserCredential", out var x);
            oUserCredential = (cUserCredential)x;
            string? token = "";
            //{
                //var options = new RestClientOptions("https://ubp.ugm.com.tr:9250")
                //{
                //  MaxTimeout = -1,
                //};
                //String pythonUrl = "https://ubp.ugm.com.tr:9250/login/token/?username=" + pUser.username + "&password=" + pUser.password + "&app=" + pUser.app + "&guid=" + pUser.guid;
                //var client = new RestClient(options);
                //var request = new RestRequest(pythonUrl, Method.Post);
                //RestResponse response = client.Execute(request);
                ////Console.WriteLine(response.Content);
                
                //cTokenForPython? oTokenForPython = Newtonsoft.Json.JsonConvert.DeserializeObject<cTokenForPython>(response.Content);
                //token = oTokenForPython?.access_token;
            //}
                
            token = iJwtTokenManager.Authenticate(oUserCredential);

            if (String.IsNullOrEmpty(token))
                return Unauthorized();

            return Ok("{\"access_token\": \"" + token + "\"}");
        }
    }
}
