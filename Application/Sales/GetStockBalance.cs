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
                var startLocal = request.Date.Date;              
                var endLocal   = startLocal.AddDays(1);

                var startUtc = DateTime.SpecifyKind(startLocal, DateTimeKind.Local).ToUniversalTime();
                var endUtc   = DateTime.SpecifyKind(endLocal,   DateTimeKind.Local).ToUniversalTime();

                var tyres = await _context.Tyres.Find(_ => true).ToListAsync(cancellationToken);

                var prodFilter  = Builders<Domain.Production>.Filter.Gte(p => p.ProductionDate, DateTime.MinValue)
                                & Builders<Domain.Production>.Filter.Lt (p => p.ProductionDate, endUtc);
                var productions = await _context.Productions.Find(prodFilter).ToListAsync(cancellationToken);

                var saleFilter  = Builders<Domain.Sale>.Filter.Gte(s => s.SaleDate, DateTime.MinValue)
                                & Builders<Domain.Sale>.Filter.Lt (s => s.SaleDate, endUtc);
                var sales       = await _context.Sales.Find(saleFilter).ToListAsync(cancellationToken);

                var stockBalances = tyres.Select(t =>
                {
                    var produced = productions.Where(p => p.Tyre?.Code == t.Code)
                                            .Sum(p => p.QuantityProduced);
                    var sold     = sales.Where(s => s.Tyre?.Code == t.Code)
                                        .Sum(s => s.QuantitySold);

                    return new StockBalanceDto
                    {
                        TyreCode = t.Code,
                        StockBalance = produced - sold
                    };
                }).ToList();

                return Result<List<StockBalanceDto>>.Success(stockBalances);
            }
        }
    }
}
