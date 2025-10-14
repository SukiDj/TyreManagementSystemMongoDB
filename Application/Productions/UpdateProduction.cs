using Application.Actions;
using Application.Core;
using Domain;
using MediatR;
using MongoDB.Driver;
using Persistence;

namespace Application.Productions
{
    public class UpdateProduction
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
            public int Shift { get; set; }
            public int QuantityProduced { get; set; }
            public DateTime Date { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly MongoDbContext _context;
            private readonly ActionLogger _actionLogger;

            public Handler(MongoDbContext context, ActionLogger actionLogger)
            {
                _context = context;
                _actionLogger = actionLogger;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var filter = Builders<Production>.Filter.Eq(p => p.Id, request.Id);
                var production = await _context.Productions.Find(filter).FirstOrDefaultAsync(cancellationToken);

                if (production == null)
                    return Result<Unit>.Failure("Proizvodnja nije pronađena.");

                production.Shift = request.Shift;
                production.QuantityProduced = request.QuantityProduced;
                production.ProductionDate = request.Date;

                await _context.Productions.ReplaceOneAsync(filter, production, cancellationToken: cancellationToken);

                await _actionLogger.LogActionAsync("UpdateProduction", $"Production updated for Id: {production.Id}");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
