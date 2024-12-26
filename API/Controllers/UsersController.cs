using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
  public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
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
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
      var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      if (username == null) return BadRequest("no username found in token");

      var user = await userRepository.GetUserByUsernameAsync(username);

      if (user == null) return BadRequest("cound not find user");

      mapper.Map(memberUpdateDto, user);
    
      if (await userRepository.SaveAllAsync()) return NoContent();

      return BadRequest("failed to update the user");

    }
  }

}