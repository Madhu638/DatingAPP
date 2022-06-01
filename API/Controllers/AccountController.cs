using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using API.Entities;
using System.Linq;
using API.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using API.DTO;
using API.Interfaces;
using API.Services;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        public readonly DataContext _Context ;
        public readonly TokenService _tokenService ;

        public AccountController(DataContext context,ITokenService tokenService)
        {
            _Context=context;
            _tokenService= (TokenService)tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(registerdto Regdto)
        {
           if (await UserExists(Regdto.username)) return BadRequest("user exist");
            using var hmac=new HMACSHA512();
            var user=new AppUser
            {
                UserName=Regdto.username.ToLower(),
                 PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(Regdto.password)),
                PasswordSalt=hmac.Key
            };

            _Context.Users.Add(user);
            await _Context.SaveChangesAsync();
            return new UserDto
            {
                Username =user.UserName,
                Token=_tokenService.createToken(user)
            };

        }

        public async Task<bool> UserExists(string username)
        {
            return await _Context.Users.AnyAsync(x =>x.UserName == username.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user=await _Context.Users.SingleOrDefaultAsync(x=>x.UserName == loginDto.username);
            if(user ==null) return Unauthorized("Invalid UserName");
           
            using var hmac=new HMACSHA512(user.PasswordSalt);
            var ComputeHas=hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));
            for(int i=0;i<ComputeHas.Length;i++)
            {
                if(ComputeHas[i] !=user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }
           return new UserDto
            {
                Username =user.UserName,
                Token=_tokenService.createToken(user)
            };
        }
    }
}