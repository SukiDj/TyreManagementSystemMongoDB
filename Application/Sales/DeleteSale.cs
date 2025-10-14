using Application.Actions;
using Application.Core;
using MediatR;
using Persistence;

namespace Application.Sales
{
    public class DeleteSale
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
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

                var saleId = sale.Id;

                _context.Sales.Remove(sale);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to delete sale");

                await _actionLogger.LogActionAsync("DeleteSale", $"Sale deleted - SaleId: {saleId}");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}