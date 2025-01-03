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
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController(DataContext context , ITokenService tokenService , IMapper mapper ) : BaseApiController
    {
        [HttpPost("register")]  // account/register 
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDTO)
        {


           if (await UserExists(registerDTO.Username)) return BadRequest("username is taken");
            using var hmac = new HMACSHA512();

var user = mapper.Map<AppUser>(registerDTO);
user.UserName= registerDTO.Username;
   user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)); 
   user.PasswordSalt = hmac.Key;


            // var user = new AppUser
            // {
            //     UserName = registerDTO.Username.ToLower(),
            //  
            //     PasswordSalt = hmac.Key

            // };

            await  context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return  new UserDto{
                Username = user.UserName,
                Token= tokenService.createToken(user),
                KnownAs= user.KnownAs,
                Gender= user.Gender,    
            };

        }

        [HttpPost("login")]

        public async Task<ActionResult<UserDto>> Login(LoginDTO loginDTO){
            var user = await context.Users.Include(p=>p.Photos).FirstOrDefaultAsync(x => 
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
                Token = tokenService.createToken(user)  ,
                 KnownAs= user.KnownAs,
                PhotoUrl = user.Photos.FirstOrDefault(x=>x.IsMain)?.URL,
                Gender= user.Gender,
             }; 
                    }

        private async Task<bool> UserExists(string Username)
        {
            return await context.Users.AnyAsync(x => x.UserName.ToLower() == Username.ToLower());
        }
    }
}