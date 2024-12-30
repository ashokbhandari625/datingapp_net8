using System.Formats.Tar;
using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Inerfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  [Authorize]
  public class UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService) : BaseApiController
  {
        [HttpGet]

    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
    {
userParams.CurrentUserName= User.GetUsername()  ;

      var users = await userRepository.GetMembersAsync(userParams); // returning memberdto 

Response.AddPaginationHeader(users);
      return Ok(users);
    }


    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
      var user = await userRepository.GetMemberAsync(username);   // member dto 

      if (user == null) return NotFound();

      return Ok(user);


    }
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
      var username = await userRepository.GetUserByUsernameAsync(User.GetUsername());
      if (username == null) return BadRequest("no username found in token");

      var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

      if (user == null) return BadRequest("cound not find user");

      mapper.Map(memberUpdateDto, user);

      if (await userRepository.SaveAllAsync()) return NoContent();

      return BadRequest("failed to update the user");

    }

    [HttpPost("add-photo")]

    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile File)
    {

      var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
      if (user == null) return BadRequest("Cannot update user");
      var results = await photoService.AddPhotoAsync(File);
      if (results.Error != null) return BadRequest(results.Error.Message);

      var photo = new Photo
      {
        URL = results.SecureUrl.AbsoluteUri,
        PulicId = results.PublicId,
      };
if(user.Photos.Count == 0) photo.IsMain = true ; 
      user.Photos.Add(photo);
      if (await userRepository.SaveAllAsync())

        return CreatedAtAction(nameof(GetUser),
        new
        { username = user.UserName }, mapper.Map<PhotoDto>(photo));




      return BadRequest("Problem adding photo");
    }

  }
}