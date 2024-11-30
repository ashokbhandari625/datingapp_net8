using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Inerfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController(DataContext context , ITokenService tokenService) : BaseApiController
    {
        [HttpPost("register")]  // account/register 
        public async Task<ActionResult<UserDto>> Register(RegisterDTO registerDTO)
        {

            if (await UserExists(registerDTO.Username)) return BadRequest("username is taken");
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = registerDTO.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt = hmac.Key

            };

            await  context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return  new UserDto{
                Username = user.UserName,
                Token= tokenService.createToken(user) 
            };

        }

        [HttpPost("login")]

        public async Task<ActionResult<UserDto>> Login(LoginDTO loginDTO){
            var user = await context.Users.FirstOrDefaultAsync(x => 
            x.UserName == loginDTO.UserName.ToLower()); 

            if (user == null) return Unauthorized("Invalid username"); 
            using var hmac = new HMACSHA512(user.PasswordSalt);
            
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            for( int i = 0 ; i<computedHash.Length ; i++ ){

                if( computedHash[i] != user.PasswordHash[i]) 
                return Unauthorized("invalid password")  ; 
              
            }
             return new UserDto{
                Username = user.UserName,   
                Token = tokenService.createToken(user)  
             }; 
                    }

        private async Task<bool> UserExists(string Username)
        {
            return await context.Users.AnyAsync(x => x.UserName.ToLower() == Username.ToLower());
        }
    }
}