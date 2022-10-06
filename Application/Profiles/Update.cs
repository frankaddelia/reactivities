using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
  public class Update
  {
    public class Command : IRequest<Result<Unit>>
    {
      public Profile Profile { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
      private readonly DataContext _context;
      private readonly IUserAccessor _userAccessor;

      public Handler(DataContext context, IUserAccessor userAccessor)
      {
        _context = context;
        _userAccessor = userAccessor;
      }

      public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
      {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());

        if (user == null) return null;

        // don't allow blank display names
        if (request.Profile.DisplayName.Length == 0)
        {
          return null;
        }

        user.DisplayName = request.Profile.DisplayName;

        user.Bio = request.Profile.Bio;

        var result = await _context.SaveChangesAsync() > 0;

        if (result)
        {
          return Result<Unit>.Success(Unit.Value);
        }

        return Result<Unit>.Failure("There was a problem updating the user profile.");
      }
    }
  }
}
