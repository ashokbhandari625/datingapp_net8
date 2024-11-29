using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class UsersController(DataContext context) : BaseApiController 
    {
        [HttpGet]

        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
          return await context.Users.ToListAsync(); 
        }
    [HttpGet("{id}")]
        public async Task< ActionResult<AppUser>> GetUser(int id ){

            if( await context.Users.FindAsync(id) == null ) return NotFound(); 
          return await  context.Users.FindAsync(id);
        }
    }

}