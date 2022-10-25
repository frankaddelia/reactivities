using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Activities;
using Application.Core;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
  public class ActivitiesController : BaseApiController
  {
    [HttpGet]
    public async Task<IActionResult> GetActivities([FromQuery] PagingParams param)
    {
      return HandlePagedResult(await Mediator.Send(new List.Query { Params = param }));
    }

    [Authorize]
    [HttpGet("{id}")] // acitivies/id
    public async Task<IActionResult> GetActivity(Guid id)
    {
      return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
    }

    [HttpPost]
    public async Task<IActionResult> CreateActivity(Activity newActivity)
    {
      return HandleResult(await Mediator.Send(new Create.Command { Activity = newActivity }));
    }

    [Authorize(Policy = "IsActivityHost")]
    [HttpPut("{id}")]
    public async Task<IActionResult> EditActivity(Guid id, [FromBody] Activity activity)
    {
      activity.Id = id;
      return HandleResult(await Mediator.Send(new Edit.Command { Activity = activity }));
    }

    [Authorize(Policy = "IsActivityHost")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(Guid id)
    {
      return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
    }

    [HttpPost("{id}/attend")]
    public async Task<IActionResult> Attend(Guid id)
    {
      return HandleResult(await Mediator.Send(new UpdateAttendence.Command { Id = id }));
    }
  }
}
