using Application.Actions;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
            private readonly DataContext _context;
            private readonly ActionLogger _actionLogger;

            public Handler(DataContext context, ActionLogger actionLogger)
            {
                _context = context;
                _actionLogger = actionLogger;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var tyre = await _context.Tyres.FirstOrDefaultAsync(t => t.Code == request.Sale.TyreId);

                if (tyre == null)
                {
                    return Result<Unit>.Failure("Invalid references for Tyre");
                }

                var client = await _context.Clients.FindAsync(request.Sale.ClientId);

                if (client == null)
                {
                    return Result<Unit>.Failure("Invalid references for Client");
                }

                var sale = new Sale
                {
                    Tyre = tyre,
                    Client = client,
                    SaleDate = request.Sale.SaleDate,
                    QuantitySold = request.Sale.QuantitySold,
                    PricePerUnit = request.Sale.PricePerUnit,
                    UnitOfMeasure = request.Sale.UnitOfMeasure,
                    TargetMarket = request.Sale.TargetMarket,
                    Production = await _context.Productions.FindAsync(request.Sale.ProductionOrderId)
                };

                if (sale.Tyre == null || sale.Client == null)
                {
                    return Result<Unit>.Failure("Invalid references for Tyre or Client");
                }

                _context.Sales.Add(sale);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to register sale");

                await _actionLogger.LogActionAsync("RegisterSale", $"Sale registered for TyreId: {request.Sale.TyreId}, ClientId: {request.Sale.ClientId}");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}