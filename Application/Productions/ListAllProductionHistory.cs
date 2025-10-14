using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Productions
{
    public class ListAllProductionHistory
    {
        public class Query : IRequest<Result<List<ProductionDto>>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<List<ProductionDto>>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<ProductionDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var history = await _context.Productions
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