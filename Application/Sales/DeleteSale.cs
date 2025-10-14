using Application.Actions;
using Application.Core;
using MediatR;
using MongoDB.Driver;
using Persistence;

namespace Application.Sales
{
    public class DeleteSale
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
                var result = await _context.Sales.DeleteOneAsync(s => s.Id == request.Id, cancellationToken);

                if (result.DeletedCount == 0)
                    return Result<Unit>.Failure("Sale not found or already deleted");

                await _actionLogger.LogActionAsync("DeleteSale", $"Sale deleted - Id: {request.Id}");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
