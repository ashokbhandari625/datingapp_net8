using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController(DataContext context) : BaseApiController
    {
        [HttpPost("register")]  // account/register
        public async Task<ActionResult<AppUser>> Register(RegisterDTO registerDTO)
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
            return  Ok(user);

        }

        private async Task<bool> UserExists(string Username)
        {
            return await context.Users.AnyAsync(x => x.UserName.ToLower() == Username.ToLower());
        }
    }
}