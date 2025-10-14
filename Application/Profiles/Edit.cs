using Application.Core;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Telefon { get; set; }
        public DateTime DatumRodjenja { get; set; }
        //public string SlikaProfila { get; set; }
        //public string UserName { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Ime).NotEmpty();
            RuleFor(x => x.Prezime).NotEmpty();
            RuleFor(x => x.Telefon).NotEmpty();
            RuleFor(x => x.DatumRodjenja).NotEmpty();
            //RuleFor(x => x.UserName).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
            _context = context;
        }
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x =>
                x.UserName == _userAccessor.GetUsername());

            user.Ime = request.Ime ?? user.Ime;
            user.Prezime = request.Prezime ?? user.Prezime;
            user.Telefon = request.Telefon ?? user.Telefon;
            //user.UserName = request.UserName ?? user.UserName;
            user.DatumRodjenja = request.DatumRodjenja;

            var success = await _context.SaveChangesAsync() > 0;

            if (success) return Result<Unit>.Success(Unit.Value);
            
            return Result<Unit>.Failure("Problem updating profile");
        }
    }
}