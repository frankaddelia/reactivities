using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {

        public ActivitiesController(IMediator mediator)
        {
            
        }

        [HttpGet]
        public async Task<ActionResult<List<Activity>>> GetActivities()
        {
            return await _context.Activities.ToListAsync();
        }

        // acitivies/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(Guid id)
        {
            return await _context.Activities.FindAsync(id);
        }

        [HttpPost("create-activity")]
        public async Task<ActionResult<Activity>> CreateActivity([FromBody] Activity newActivity)
        {
            try
            {
                await _context.Activities.AddAsync(newActivity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was a problem creating a new action.", ex);
            }

            return NoContent();
        }
    }
}