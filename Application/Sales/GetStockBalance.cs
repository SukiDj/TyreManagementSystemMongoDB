using Application.Core;
using MediatR;
using MongoDB.Driver;
using Persistence;

namespace Application.Sales
{
    public class GetStockBalance
    {
        public class Query : IRequest<Result<List<StockBalanceDto>>>
        {
            public DateTime Date { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<StockBalanceDto>>>
        {
            private readonly MongoDbContext _context;

            public Handler(MongoDbContext context)
            {
                _context = context;
            }

            public async Task<Result<List<StockBalanceDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var tyres = await _context.Tyres.Find(_ => true).ToListAsync(cancellationToken);
                var productions = await _context.Productions
                    .Find(p => p.ProductionDate <= request.Date)
                    .ToListAsync(cancellationToken);

                var sales = await _context.Sales
                    .Find(s => s.SaleDate <= request.Date)
                    .ToListAsync(cancellationToken);

                var stockBalances = tyres.Select(t =>
                {
                    var produced = productions.Where(p => p.Tyre.Id == t.Id).Sum(p => p.QuantityProduced);
                    var sold = sales.Where(s => s.Tyre.Id == t.Id).Sum(s => s.QuantitySold);

                    return new StockBalanceDto
                    {
                        TyreCode = t.Id.ToString(),
                        StockBalance = produced - sold
                    };
                }).ToList();

                return Result<List<StockBalanceDto>>.Success(stockBalances);
            }
        }
    }
}
