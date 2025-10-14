using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using MongoDB.Driver;
using Persistence;

namespace Application.Profiles
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Ime { get; set; }
            public string Prezime { get; set; }
            public string Telefon { get; set; }
            public DateTime DatumRodjenja { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Ime).NotEmpty();
                RuleFor(x => x.Prezime).NotEmpty();
                RuleFor(x => x.Telefon).NotEmpty();
                RuleFor(x => x.DatumRodjenja).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly MongoDbContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(MongoDbContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var filter = Builders<User>.Filter.Eq(u => u.Username, _userAccessor.GetUsername());
                var user = await _context.Users.Find(filter).FirstOrDefaultAsync(cancellationToken);

                if (user == null)
                    return Result<Unit>.Failure("Korisnik nije pronađen.");

                user.Ime = request.Ime ?? user.Ime;
                user.Prezime = request.Prezime ?? user.Prezime;
                user.Telefon = request.Telefon ?? user.Telefon;
                user.DatumRodjenja = request.DatumRodjenja;

                await _context.Users.ReplaceOneAsync(filter, user, cancellationToken: cancellationToken);

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
