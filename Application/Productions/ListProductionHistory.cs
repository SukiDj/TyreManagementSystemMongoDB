using Application.Core;
using Application.Interfaces;
using MediatR;
using MongoDB.Driver;
using Domain;
using Persistence;

namespace Application.Productions
{
    public class ListProductionHistory
    {
        public class Query : IRequest<Result<List<ProductionDto>>> { }

        public class Handler : IRequestHandler<Query, Result<List<ProductionDto>>>
        {
            private readonly MongoDbContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(MongoDbContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Result<List<ProductionDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var username = _userAccessor.GetUsername();
                if (string.IsNullOrEmpty(username))
                    return Result<List<ProductionDto>>.Failure("User not found");

                var productionOperator = await _context.Users.Find(u => u.Username == username).FirstOrDefaultAsync(cancellationToken);
                if (productionOperator == null)
                    return Result<List<ProductionDto>>.Failure("Production operator not found");

                var filter = Builders<Production>.Filter.Eq(p => p.Operator.Id, productionOperator.Id);
                var productions = await _context.Productions.Find(filter).ToListAsync(cancellationToken);

                var history = productions.Select(p => new ProductionDto
                {
                    Id = p.Id,
                    TyreCode = p.Tyre?.Code,
                    QuantityProduced = p.QuantityProduced,
                    ProductionDate = p.ProductionDate,
                    Shift = p.Shift,
                    MachineNumber = p.Machine?.Id,
                    OperatorId = p.Operator?.Id
                }).ToList();

                return Result<List<ProductionDto>>.Success(history);
            }
        }
    }
}
