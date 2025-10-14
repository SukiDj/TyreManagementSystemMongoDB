using Application.Actions;
using Application.Core;
using MediatR;
using Persistence;

namespace Application.Productions
{
    public class DeleteProduction
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
                var production = await _context.Productions.FindAsync(request.Id);

                if (production == null) return null;

                var productionId = production.Id;

                _context.Productions.Remove(production);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to delete production");

                await _actionLogger.LogActionAsync("DeleteProduction", $"Production deleted - ProductionId: {productionId}");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}