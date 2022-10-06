using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  public class ProfilesController : BaseApiController
  {
    [HttpGet("{username}")]
    public async Task<IActionResult> GetProfile(string username)
    {
      return HandleResult(await Mediator.Send(new Details.Query { Username = username }));
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] Profile profile)
    {
      return HandleResult(await Mediator.Send(new Update.Command { Profile = profile }));
    }
  }
}