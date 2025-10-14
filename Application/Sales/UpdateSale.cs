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
                var sale = await _context.Sales.Find(s => s.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
                if (sale == null) return Result<Unit>.Failure("Sale not found");

                var tyre = await _context.Tyres.Find(t => t.Id == request.TyreId).FirstOrDefaultAsync(cancellationToken);
                var client = await _context.Clients.Find(c => c.Id == request.ClientId).FirstOrDefaultAsync(cancellationToken);

                if (tyre == null || client == null)
                    return Result<Unit>.Failure("Invalid references for Tyre or Client");

                sale.Tyre = tyre;
                sale.Client = client;
                sale.QuantitySold = request.QuantitySold;
                sale.PricePerUnit = request.PricePerUnit;
                sale.SaleDate = request.SaleDate;

                await _context.Sales.ReplaceOneAsync(s => s.Id == request.Id, sale, cancellationToken: cancellationToken);

                await _actionLogger.LogActionAsync("UpdateSale", $"Sale updated for Id: {sale.Id}");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
