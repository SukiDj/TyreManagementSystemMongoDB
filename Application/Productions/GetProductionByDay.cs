using Application.Core;
using MediatR;
using MongoDB.Driver;
using Domain;
using Persistence;

namespace Application.Productions
{
    public class GetProductionByDay
    {
        public class Query : IRequest<Result<List<ProductionDto>>>
        {
            public DateTime Date { get; set; }
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
                var startLocal = request.Date.Date;

                var startUtc = DateTime.SpecifyKind(startLocal, DateTimeKind.Local).ToUniversalTime();
                var endUtc   = startUtc.AddDays(1);

                var filter = Builders<Production>.Filter.Gte(p => p.ProductionDate, startUtc) &
                    Builders<Production>.Filter.Lt (p => p.ProductionDate, endUtc);
                var productions = await _context.Productions.Find(filter).ToListAsync(cancellationToken);

                var result = productions.Select(p => new ProductionDto
                {
                    Id = p.Id,
                    TyreCode = p.Tyre?.Code,
                    Shift = p.Shift,
                    QuantityProduced = p.QuantityProduced,
                    MachineNumber = p.Machine?.Id,
                    ProductionDate = p.ProductionDate,
                    OperatorId = p.Operator?.Id
                }).ToList();

                return Result<List<ProductionDto>>.Success(result);
            }
        }
    }
}
