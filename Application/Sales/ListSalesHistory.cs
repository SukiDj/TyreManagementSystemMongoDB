using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Sales
{
    public class ListSalesHistory
    {
        public class Query : IRequest<Result<List<SaleDto>>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<List<SaleDto>>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<SaleDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var salesHistory = await _context.Sales
                    .Include(s => s.Tyre)
                    .Include(s => s.Client)
                    .Select(s => new SaleDto
                    {
                        Id = s.Id,
                        TyreCode = s.Tyre.Code,
                        QuantitySold = s.QuantitySold,
                        SaleDate = s.SaleDate,
                        ClientId = s.Client.Id,
                        TargetMarket = s.TargetMarket,
                        PricePerUnit = s.PricePerUnit,
                        ProductionOrderId = s.Production.Id
                    })
                    .ToListAsync();

                return Result<List<SaleDto>>.Success(salesHistory);
            }
        }
    }
}