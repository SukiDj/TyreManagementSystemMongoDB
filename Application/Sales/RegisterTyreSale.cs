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
                var production = await _context.Productions
                    .Find(p => p.Id == request.Sale.ProductionOrderId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (production == null)
                    return Result<Unit>.Failure($"Invalid Production reference: {request.Sale.ProductionOrderId}");

                if (production.Tyre == null || string.IsNullOrWhiteSpace(production.Tyre.Code))
                    return Result<Unit>.Failure("Production is missing tyre reference.");

                var client = await _context.Clients
                    .Find(c => c.Id == request.Sale.ClientId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (client == null)
                    return Result<Unit>.Failure($"Invalid Client reference: {request.Sale.ClientId}");

                //Available quantity for THIS production order
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

                //Delivery creating with location calculation
                var machine = await _context.Machines
                    .Find(m => m.Id == production.Machine.Id)
                    .FirstOrDefaultAsync(cancellationToken);
                if (machine == null)
                    return Result<Unit>.Failure($"Production machine not found: {production.Machine.Id}");
                if (machine?.Location?.Coordinates is not { Length: 2 })
                    return Result<Unit>.Failure("Machine has no location");

                var origin = new BsonDocument
                {
                    { "type", "Point" },
                    { "coordinates", new BsonArray(machine.Location.Coordinates) }
                };

                var geoNear = new BsonDocument("$geoNear", new BsonDocument
                {
                    { "near", origin },
                    { "key", "location" },     // field in ClientLocations
                    { "spherical", true },
                    { "distanceField", "distanceMeters" },           
                    { "query", new BsonDocument("clientId", new ObjectId(client.Id)) }
                });

                var projection = new BsonDocument("$project", new BsonDocument
                {
                    { "_id", 1 },
                    { "location", 1 },
                    { "distanceMeters", 1 }
                });

                var limit = new BsonDocument("$limit", 1);

                var pick = await _context.ClientLocations
                    .Aggregate<BsonDocument>(new[] { geoNear, projection, limit })
                    .FirstOrDefaultAsync(cancellationToken);

                if (pick == null)
                    return Result<Unit>.Failure("No client locations found for nearest search");

                var destCoords = pick["location"]["coordinates"].AsBsonArray.Select(x => x.ToDouble()).ToArray();
                var destination = new GeoPoint { Type = "Point", Coordinates = destCoords };
                var distanceMeters = pick["distanceMeters"].ToDouble();

                var delivery = new Delivery
                {
                    Client = client.Name,
                    Origin = machine.Location,
                    Destination = destination,
                    DistanceMeters = distanceMeters,
                    Status = "Pending"
                };
                
                await _context.Sales.InsertOneAsync(sale, cancellationToken: cancellationToken);

                await _actionLogger.LogActionAsync(
                    "RegisterSale",
                    $"Sale registered for Production: {production.Id}, Tyre: {production.Tyre.Code}, Client: {client.Id}, Qty: {request.Sale.QuantitySold}"
                );

                await _context.Deliveries.InsertOneAsync(delivery, cancellationToken: cancellationToken);
                await _actionLogger.LogActionAsync(
                    "CreateDelivery",
                    $"Delivery created for sale {sale.Id} for Client {client.Name}");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
