using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using MmuAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using WireMock.Admin.Mappings;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using MusteriMobilUygulamaAPI.Common;
using RestSharp;
using MusteriMobilUygulamaAPI.Models.Api;
using MusteriMobilUygulamaAPI.Services.Api;

namespace MmuAPI
{
    public class CheckToken : TypeFilterAttribute
    {
        public CheckToken() : base(typeof(TokenFilterImpl))
        {
        }
        public class TokenFilterImpl : IAsyncActionFilter
        {
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                string path = context.HttpContext.Request.Path;

                if (   path == "/intranet/tanitim-video"
                    || path == "/intranet/katalog"
                    || path == "/intranet/kurumsal-derecelendirme"
                    || path == "/intranet/mali-denetim"
                    || path == "/intranet/oda-dernek"
                    || path == "/intranet/etik-kurallar"
                    || path == "/api/emobilBlog"
                    || path == "/api/emobilBlogLikes"
                    || path == "/api/KullaniciKaydi"
                    || path == "/api/Dil"
                    || path == "/api/emobilSoruCevap"
                    || path == "/api/emobilSoruCevapTip"
                    || path == "/api/emobilOfis"
                    || path == "/api/send-mail"
                    || path == "/api/kys-belgeleri"
                    || path == "/api/MailAuth"
                    )
                {
                    await next();
                    return;
                }

                
                JwtSecurityToken jwtSecurityToken;
                try
                {

                    var token = context.HttpContext.Request.Headers[HeaderNames.Authorization];
                    token = token.ToString().Replace("Bearer ", "");
                    var accessTokens = context.HttpContext.Request.Headers;

                    //string? token = context.HttpContext.GetTokenAsync("access_token").Result;
                    jwtSecurityToken = new JwtSecurityToken(token);
                    if (jwtSecurityToken.ValidTo < DateTime.UtcNow)
                    {
                        context.Result = new UnauthorizedObjectResult("Unauthorized");  //Unauthorized();
                        return;
                    }
                    cUserCredential oUserCredential = new cUserCredential();

                    oUserCredential.in_user_id = Int32.Parse(getJWTTokenClaim(token, "in_user_id"));
                    oUserCredential.ex_user_id = Int32.Parse(getJWTTokenClaim(token, "ex_user_id"));
                        
                    oUserCredential.internalx = true;
                    if (oUserCredential.in_user_id == -1) oUserCredential.internalx = false;
                        
                    oUserCredential.externalx = true;
                    if (oUserCredential.ex_user_id == -1) oUserCredential.externalx = false;
                        
                    oUserCredential.email = getJWTTokenClaim(token, "email");
                    oUserCredential.imei = getJWTTokenClaim(token, "imei");
                    context.HttpContext.Items["oUserCredential"] = oUserCredential;

                    _ = LoggerX.LogIn(context.HttpContext, "");
                    //Task.Run(() =>  LoggerX.LogIn(context.HttpContext, "") );
                }
                catch (Exception pExc)
                {
                    Exception exc = pExc;
                    context.Result = new UnauthorizedObjectResult("Unauthorized");  //Unauthorized();
                    _ = LoggerX.LogIn(context.HttpContext, "");
                    //Task.Run(() =>  LoggerX.LogIn(context.HttpContext, "") );
                    return;
                }                

                await next();
            }
            public string? getJWTTokenClaim(string token, string claimName)
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                    var claimValue = securityToken.Claims.FirstOrDefault(c => c.Type == claimName)?.Value;
                    return claimValue == "" ? "-1" : claimValue;
                }
                catch (Exception)
                {
                    //TODO: Logger.Error
                    return "";
                }
            }
        }
    }
}
