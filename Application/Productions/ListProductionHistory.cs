using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Productions
{
    public class ListProductionHistory
    {
        public class Query : IRequest<Result<List<ProductionDto>>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<List<ProductionDto>>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Result<List<ProductionDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var productionOperator = _context.ProductionOperators.FirstOrDefault(x => 
                    x.UserName == _userAccessor.GetUsername());

                var history = await _context.Productions
                    .Where(p => p.Operator.Id == productionOperator.Id)
                    .Select(p => new ProductionDto
                    {
                        Id = p.Id,
                        TyreCode = p.Tyre.Code.ToString(),
                        QuantityProduced = p.QuantityProduced,
                        ProductionDate = p.ProductionDate,
                        Shift = p.Shift,
                        MachineNumber = p.Machine.Id.ToString(),
                        OperatorId = p.Operator.Id.ToString()
                    })
                    .ToListAsync();

                return Result<List<ProductionDto>>.Success(history);
            }
        }
    }
}