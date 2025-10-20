using Application.Core;
using MediatR;
using MongoDB.Driver;
using Domain;
using Persistence;

namespace Application.Productions
{
    public class ListAllProductionHistory
    {
        public class Query : IRequest<Result<List<ProductionDto>>> { }

        public class Handler : IRequestHandler<Query, Result<List<ProductionDto>>>
        {
            private readonly MongoDbContext _context;

            public Handler(MongoDbContext context)
            {
                _context = context;
            }

            public async Task<Result<List<ProductionDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var productions = await _context.Productions.Find(_ => true).ToListAsync(cancellationToken);

                var history = productions.Select(p => new ProductionDto
                {
                    Id = p.Id,
                    TyreCode = p.Tyre?.Code,
                    QuantityProduced = p.QuantityProduced,
                    ProductionDate = p.ProductionDate,
                    Shift = p.Shift,
                    MachineNumber = p.Machine?.Id,
                    OperatorId = p.Operator?.Id,
                    MachineName = p.Machine?.Name,
                    OperatorName = p.Operator?.Name,
                    TyreType = p.Tyre?.Name
                }).ToList();

                return Result<List<ProductionDto>>.Success(history);
            }
        }
    }
}
