using System;
using System.Text;
using API.Interfaces;
using API.Entities;
using System.Text;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key; 
        public TokenService(IConfiguration config)
        {
            _key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public string createToken(AppUser user)
        {
           var claims=new List<Claim>
           {
               new Claim(JwtRegisteredClaimNames.NameId,user.UserName)
           };
           var creds=new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
           var tokenDiscriptor=new SecurityTokenDescriptor
           {
               Subject= new ClaimsIdentity(claims),
               Expires =DateTime.Now.AddDays(7),
               SigningCredentials = creds
           };

           var tokenHandler=new JwtSecurityTokenHandler();
           var token=tokenHandler.CreateToken(tokenDiscriptor);

           return tokenHandler.WriteToken(token);
        }
    }
}