using Application.Actions;
using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Productions
{
    public class RegisterProduction
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid TyreId { get; set; }
            public Guid MachineId { get; set; }
            public int Shift { get; set; }
            public int QuantityProduced { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly ActionLogger _actionLogger;
            public Handler(DataContext context, IUserAccessor userAccessor, ActionLogger actionLogger)
            {
                _context = context;
                _userAccessor = userAccessor;
                _actionLogger = actionLogger;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var productionOperator = _context.ProductionOperators.FirstOrDefault(x => 
                    x.UserName == _userAccessor.GetUsername());

                if(productionOperator ==null) return Result<Unit>.Failure("Nije pronadjen operater!");

                var machine = await _context.Machines.FindAsync(request.MachineId);
                
                var tyre = await _context.Tyres.FirstOrDefaultAsync(t => t.Code == request.TyreId);

                var production = new Production
                {
                    Tyre = tyre,
                    Operator = productionOperator,
                    //Supervisor = supervisor,
                    Machine = machine,
                    Shift = request.Shift,
                    QuantityProduced = request.QuantityProduced,
                    ProductionDate = DateTime.UtcNow
                };

                if (production.Tyre == null || production.Operator == null || production.Machine == null)
                {
                    return Result<Unit>.Failure("Invalid references for Tyre, Operator, or Machine");
                }

                _context.Productions.Add(production);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to register production");

                await _actionLogger.LogActionAsync("RegisterProduction", $"Production registered for TyreId: {request.TyreId}, OperatorId: {productionOperator}");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}