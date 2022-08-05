using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Activities;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
  public class ActivitiesController : BaseApiController
  {
    [HttpGet]
    public async Task<ActionResult<List<Activity>>> GetActivities()
    {
      return await Mediator.Send(new List.Query());
    }

    // acitivies/id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetActivity(Guid id)
    {
      var result = await Mediator.Send(new Details.Query { Id = id });

      if (result.IsSuccess && result.Value != null)
      {
        return Ok(result.Value);
      }

      if (result.IsSuccess && result.Value == null)
      {
        return NotFound();
      }

      return BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> CreateActivity(Activity newActivity)
    {
      return Ok(await Mediator.Send(new Create.Command { Activity = newActivity }));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditActivity(Guid id, [FromBody] Activity activity)
    {
      activity.Id = id;
      return Ok(await Mediator.Send(new Edit.Command { Activity = activity }));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(Guid id)
    {
      return Ok(await Mediator.Send(new Delete.Command { Id = id }));
    }
  }
}