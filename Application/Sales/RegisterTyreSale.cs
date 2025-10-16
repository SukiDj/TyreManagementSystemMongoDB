using Application.Actions;
using Application.Core;
using Domain;
using MediatR;
using MongoDB.Bson;
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
                // 1) Production must exist
                var production = await _context.Productions
                    .Find(p => p.Id == request.Sale.ProductionOrderId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (production == null)
                    return Result<Unit>.Failure($"Invalid Production reference: {request.Sale.ProductionOrderId}");

                if (production.Tyre == null || string.IsNullOrWhiteSpace(production.Tyre.Code))
                    return Result<Unit>.Failure("Production is missing tyre reference.");

                // 2) Client must exist
                var client = await _context.Clients
                    .Find(c => c.Id == request.Sale.ClientId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (client == null)
                    return Result<Unit>.Failure($"Invalid Client reference: {request.Sale.ClientId}");

                // (Optional) If DTO still has TyreId, enforce it matches production's tyre
                // if (!string.IsNullOrEmpty(request.Sale.TyreId) && request.Sale.TyreId != production.Tyre.Code)
                //     return Result<Unit>.Failure("Selected Tyre does not match the Production's tyre.");

                // 3) Compute available quantity for THIS production order
                var existingSales = await _context.Sales
                    .Find(s => s.ProductionId == production.Id)
                    .Project(s => s.QuantitySold)
                    .ToListAsync(cancellationToken);

                var alreadySold = existingSales.Sum();
                var available = production.QuantityProduced - alreadySold;

                if (request.Sale.QuantitySold > available)
                {
                    return Result<Unit>.Failure(
                        $"Not enough stock in production order {production.Id}. " +
                        $"Available: {available}, requested: {request.Sale.QuantitySold}."
                    );
                }


                // 5) Create sale using tyre info from PRODUCTION
                var sale = new Sale
                {
                    Tyre = new TyreInfo
                    {
                        Code = production.Tyre.Code,
                        Name = production.Tyre.Name
                    },
                    Client = new ClientInfo
                    {
                        Id = client.Id,
                        Name = client.Name
                    },
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
                    $"Sale registered for Production: {production.Id}, Tyre: {production.Tyre.Code}, Client: {client.Id}, Qty: {request.Sale.QuantitySold}"
                );

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
