using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(AppUser user)
        {
            //Adding claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                // new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString())
            };
            // Creating Signatures
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
            //creating the token skeleton
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };
            //creating token handler
            var tokenHandler = new JwtSecurityTokenHandler();
            //creating the token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //returning the written token
            return tokenHandler.WriteToken(token);
        }
    }
}