using Application.Actions;
using Application.Core;
using MediatR;
using Persistence;

namespace Application.Sales
{
    public class UpdateSale
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
            public Guid TyreId { get; set; }
            public Guid ClientId { get; set; }
            public int QuantitySold { get; set; }
            public double PricePerUnit { get; set; }
            public DateTime SaleDate { get; set; }
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
                var sale = await _context.Sales.FindAsync(request.Id);

                if (sale == null) return null;

                sale.Tyre = await _context.Tyres.FindAsync(request.TyreId);
                sale.Client = await _context.Clients.FindAsync(request.ClientId);
                sale.QuantitySold = request.QuantitySold;
                sale.PricePerUnit = request.PricePerUnit;
                sale.SaleDate = request.SaleDate;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to update sale");

                await _actionLogger.LogActionAsync("UpdateSale", $"Sale updated for TyreId: {request.TyreId}, SaleId: {sale.Id}");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}