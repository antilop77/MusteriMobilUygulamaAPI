using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MmuAPI.Models;

namespace MmuAPI
{
    public class JwtTokenManager : IJwtTokenManager
    {
        private readonly IConfiguration iConfiguration;
        public JwtTokenManager(IConfiguration pIConfiguration)
        {
            iConfiguration = pIConfiguration;
        }
        public string Authenticate(cUserCredential pUserCredential)
        {            
            int pExpirationMinutes = Int32.Parse(iConfiguration.GetValue<string>("Config:sExpirationMinutes"));

            string key = iConfiguration.GetValue<string>("JwtConfig:Key");
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                      new Claim("in_user_id", pUserCredential.in_user_id.ToString())
                    , new Claim("ex_user_id", pUserCredential.ex_user_id.ToString())
                    , new Claim("imei", pUserCredential.imei == null ? "" : pUserCredential.imei)
                    , new Claim("email", pUserCredential.email == null ? "" : pUserCredential.email)
                    , new Claim("internal", pUserCredential.internalx.ToString(), ClaimValueTypes.Boolean)
                    , new Claim("external", pUserCredential.externalx.ToString(), ClaimValueTypes.Boolean)
                })
                ,
                Expires = DateTime.UtcNow.AddMinutes(pExpirationMinutes)
                ,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);


            string strToken = tokenHandler.WriteToken(token);
            return strToken;
        }
    }
}
