using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Persistence;
using Domain;

namespace Application.Activities
{
  public class UpdateAttendence
  {
    public class Command : IRequest<Result<Unit>>
    {
      public Guid Id { get; set; }

    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
      private readonly IUserAccessor _userAccessor;
      private readonly DataContext _context;
      public Handler(DataContext context, IUserAccessor userAccessor)
      {
        _context = context;
        _userAccessor = userAccessor;
      }

      public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
      {
        var activity = await _context.Activities
            .Include(a => a.Attendees).ThenInclude(u => u.AppUser)
            .SingleOrDefaultAsync(x => x.Id == request.Id);

        if (activity == null)
        {
          return null;
        }

        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

        if (user == null)
        {
          return null;
        }

        var hostUserName = activity.Attendees.FirstOrDefault(x => x.IsHost)?.AppUser?.UserName;

        var attendence = activity.Attendees.FirstOrDefault(x => x.AppUser.UserName == user.UserName);

        if (attendence != null && hostUserName == user.UserName)
        {
          activity.IsCancelled = !activity.IsCancelled;
        }

        if (attendence != null && hostUserName != user.UserName)
        {
          activity.Attendees.Remove(attendence);
        }

        if (attendence == null)
        {
          attendence = new ActivityAttendee
          {
            AppUser = user,
            Activity = activity,
            IsHost = false
          };

          activity.Attendees.Add(attendence);
        }

        var result = await _context.SaveChangesAsync() > 0;

        return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem updating attendence.");
      }
    }
  }
}