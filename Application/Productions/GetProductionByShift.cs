using Application.Core;
using MediatR;
using MongoDB.Driver;
using Domain;
using Persistence;

namespace Application.Productions
{
    public class GetProductionByShift
    {
        public class Query : IRequest<Result<List<ProductionDto>>>
        {
            public int Shift { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<ProductionDto>>>
        {
            private readonly MongoDbContext _context;

            public Handler(MongoDbContext context)
            {
                _context = context;
            }

            public async Task<Result<List<ProductionDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var filter = Builders<Production>.Filter.Eq(p => p.Shift, request.Shift);
                var productions = await _context.Productions.Find(filter).ToListAsync(cancellationToken);

                var result = productions.Select(p => new ProductionDto
                {
                    Id = p.Id,
                    TyreCode = p.Tyre?.Code,
                    Shift = p.Shift,
                    QuantityProduced = p.QuantityProduced,
                    MachineNumber = p.Machine?.Id,
                    ProductionDate = p.ProductionDate,
                    OperatorId = p.Operator?.Id,
                    MachineName = p.Machine.Name,
                    OperatorName = p.Operator.Name,
                    TyreType = p.Tyre.Name
                }).ToList();

                return Result<List<ProductionDto>>.Success(result);
            }
        }
    }
}
