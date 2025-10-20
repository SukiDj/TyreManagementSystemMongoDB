using Application.Actions;
using Application.Core;
using Domain;
using MediatR;
using MongoDB.Driver;
using Persistence;

namespace Application.Sales
{
    public class UpdateSale
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
            public string TyreId { get; set; }
            public string ClientId { get; set; }
            public int QuantitySold { get; set; }
            public double PricePerUnit { get; set; }
            public DateTime SaleDate { get; set; }
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
                var filter = Builders<Sale>.Filter.Eq(s => s.Id, request.Id);

                var update = Builders<Sale>.Update.Combine(
                    Builders<Sale>.Update.Set(s => s.QuantitySold, request.QuantitySold),
                    Builders<Sale>.Update.Set(s => s.PricePerUnit, request.PricePerUnit)
                );

                var result = await _context.Sales.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

                if (result.MatchedCount == 0)
                    return Result<Unit>.Failure("Sale not found");

                await _actionLogger.LogActionAsync("UpdateSale", $"Sale updated for Id: {request.Id}");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
