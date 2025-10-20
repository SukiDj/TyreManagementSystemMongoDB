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
            public string Id { get; set; } = default!;
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

                var update = Builders<Production>.Update
                    .Set(p => p.Shift, request.Shift)
                    .Set(p => p.QuantityProduced, request.QuantityProduced);

                var result = await _context.Productions.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

                if (result.MatchedCount == 0)
                    return Result<Unit>.Failure("Production record not found.");

                await _actionLogger.LogActionAsync("UpdateProduction", $"Production updated for Id: {request.Id}");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
