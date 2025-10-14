using Application.Core;
using Domain;
using MediatR;
using MongoDB.Driver;
using Persistence;

namespace Application.Sales
{
    public class ListSalesHistory
    {
        public class Query : IRequest<Result<List<SaleDto>>> { }

        public class Handler : IRequestHandler<Query, Result<List<SaleDto>>>
        {
            private readonly MongoDbContext _context;

            public Handler(MongoDbContext context)
            {
                _context = context;
            }

            public async Task<Result<List<SaleDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var sales = await _context.Sales.Find(_ => true).ToListAsync(cancellationToken);

                var salesHistory = sales.Select(s => new SaleDto
                {
                    Id = s.Id,
                    TyreCode = s.Tyre?.Id ?? Guid.Empty,
                    QuantitySold = s.QuantitySold,
                    SaleDate = s.SaleDate,
                    ClientId = s.Client?.Id ?? Guid.Empty,
                    TargetMarket = s.TargetMarket,
                    PricePerUnit = s.PricePerUnit,
                    ProductionOrderId = s.Production?.Id ?? Guid.Empty
                }).ToList();

                return Result<List<SaleDto>>.Success(salesHistory);
            }
        }
    }
}
