using Application.Actions;
using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using MongoDB.Driver;
using Persistence;

namespace Application.Productions
{
    public class RegisterProduction
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string TyreId { get; set; }
            public string MachineId { get; set; }
            public int Shift { get; set; }
            public int QuantityProduced { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.TyreId).NotEmpty();
                RuleFor(x => x.MachineId).NotEmpty();
                RuleFor(x => x.QuantityProduced).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly MongoDbContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly ActionLogger _actionLogger;

            public Handler(MongoDbContext context, IUserAccessor userAccessor, ActionLogger actionLogger)
            {
                _context = context;
                _userAccessor = userAccessor;
                _actionLogger = actionLogger;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var productionOperator = await _context.ProductionOperators
                    .Find(x => x.UserName == _userAccessor.GetUsername())
                    .FirstOrDefaultAsync(cancellationToken);

                if (productionOperator == null)
                    return Result<Unit>.Failure("Nije pronađen operater!");

                var machine = await _context.Machines
                    .Find(x => x.Id == request.MachineId)
                    .FirstOrDefaultAsync(cancellationToken);

                var tyre = await _context.Tyres
                    .Find(x => x.Id == request.TyreId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (tyre == null || machine == null)
                    return Result<Unit>.Failure("Nisu pronađeni svi potrebni entiteti.");

                var production = new Production
                {
                    Tyre = tyre,
                    Operator = productionOperator,
                    Machine = machine,
                    Shift = request.Shift,
                    QuantityProduced = request.QuantityProduced,
                    ProductionDate = DateTime.UtcNow
                };

                await _context.Productions.InsertOneAsync(production, cancellationToken: cancellationToken);

                await _actionLogger.LogActionAsync("RegisterProduction",
                    $"Production registered for TyreId: {request.TyreId}, Operator: {productionOperator.UserName}");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
