using Application.Actions;
using Application.Core;
using MediatR;
using MongoDB.Driver;
using Domain;
using Persistence;

namespace Application.Productions
{
    public class DeleteProduction
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
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
                var production = await _context.Productions.Find(p => p.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

                if (production == null) return Result<Unit>.Failure("Production not found");

                var productionId = production.Id;

                var deleteResult = await _context.Productions.DeleteOneAsync(p => p.Id == request.Id, cancellationToken);

                if (deleteResult.DeletedCount == 0)
                    return Result<Unit>.Failure("Failed to delete production");

                await _actionLogger.LogActionAsync("DeleteProduction", $"Production deleted - ProductionId: {productionId}");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
