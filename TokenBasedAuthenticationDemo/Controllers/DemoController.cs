using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TokenBasedAuthenticationDemo.Extensions;
using TokenBasedAuthenticationDemo.Model;

namespace TokenBasedAuthenticationDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly TokenAuthOptions tokenOptions;

        public DemoController(IOptions<TokenAuthOptions> tokenOptions)
        {
            this.tokenOptions = tokenOptions.Value;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public string GetDemo()=>"Hello World!"; 
        
        [HttpPost]
        public GeneratedToken CreateCredentials([FromBody] UserModel user)
        {
            if(!Autenticate(user))
            {
                return null;
            }
            var tokenStr = GenerateJsonWebToken(user);
            return new GeneratedToken { Bearer = tokenStr };
        }

        private string GenerateJsonWebToken(UserModel user)
        {
            //nuget package Microsoft.Identity.Models.Tokens
            //var keyBytes = Encoding.UTF8.GetBytes(tokenOptions.Key);
            //var securityKey = new SymmetricSecurityKey(keyBytes);
            var securityKey = tokenOptions.ExtractSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                //nuget package Microsoft.IdentityModels.Tokens.Jwt
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(tokenOptions.MinutesOfLife),
                signingCredentials: credentials);
            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
            //Now go to startup to configure cors
        }

        private bool Autenticate(UserModel user)
        {
            return true;
        }
    }
}