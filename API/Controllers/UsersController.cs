using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Inerfaces;
using AutoMapper;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
 [Authorize]
  public class UsersController(IUserRepository userRepository) : BaseApiController
  {

   

    [HttpGet]

    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {

      var users = await userRepository.GetMembersAsync(); // returning memberdto 


      return Ok(users);
    }


    [HttpGet("getuser")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
      var user = await userRepository.GetMemberAsync(username);   // member dto 

      if (user == null) return NotFound();


      return Ok(user);


    }
  }

}