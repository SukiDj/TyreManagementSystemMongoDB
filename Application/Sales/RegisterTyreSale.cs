using Application.Actions;
using Application.Core;
using Domain;
using MediatR;
using MongoDB.Driver;
using Persistence;

namespace Application.Sales
{
    public class RegisterTyreSale
    {
        public class Command : IRequest<Result<Unit>>
        {
            public RegisterTyreSaleDto Sale { get; set; }
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
                var tyre = await _context.Tyres
                    .Find(t => t.Id == request.Sale.TyreId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (tyre == null)
                    return Result<Unit>.Failure("Invalid Tyre reference");

                var client = await _context.Clients
                    .Find(c => c.Id == request.Sale.ClientId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (client == null)
                    return Result<Unit>.Failure("Invalid Client reference");

                var production = await _context.Productions
                    .Find(p => p.Id == request.Sale.ProductionOrderId)
                    .FirstOrDefaultAsync(cancellationToken);

                var sale = new Sale
                {
                    Tyre = tyre,
                    Client = client,
                    SaleDate = request.Sale.SaleDate,
                    QuantitySold = request.Sale.QuantitySold,
                    PricePerUnit = request.Sale.PricePerUnit,
                    UnitOfMeasure = request.Sale.UnitOfMeasure,
                    TargetMarket = request.Sale.TargetMarket,
                    ProductionId = production.Id
                };

                await _context.Sales.InsertOneAsync(sale, cancellationToken: cancellationToken);

                await _actionLogger.LogActionAsync(
                    "RegisterSale",
                    $"Sale registered for TyreId: {request.Sale.TyreId}, ClientId: {request.Sale.ClientId}"
                );

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
