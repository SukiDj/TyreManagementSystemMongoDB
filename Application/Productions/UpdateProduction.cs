using Application.Actions;
using Application.Core;
using MediatR;
using Persistence;

namespace Application.Productions
{
    public class UpdateProduction
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
            public int Shift { get; set; }
            public int QuantityProduced { get; set; }
            public DateTime Date { get; set; }
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
                var production = await _context.Productions.FindAsync(request.Id);

                if (production == null) return null;

                production.Shift = request.Shift;
                production.QuantityProduced = request.QuantityProduced;
                production.ProductionDate = request.Date;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to update production");

                await _actionLogger.LogActionAsync("UpdateProduction", $"Production updated for ProductionId: {production.Id}");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}