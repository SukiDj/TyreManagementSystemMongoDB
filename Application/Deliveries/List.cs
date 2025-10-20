using Application.Core;
using Domain;
using MediatR;
using MongoDB.Driver;
using Persistence;

namespace Application.Deliveries
{
    public class List
    {
        public class Query : IRequest<Result<List<Delivery>>> { }

        public class Handler : IRequestHandler<Query, Result<List<Delivery>>>
        {
            private readonly MongoDbContext _db;
            public Handler(MongoDbContext db) { _db = db; }

            public async Task<Result<List<Delivery>>> Handle(Query request, CancellationToken ct)
            {
                var items = await _db.Deliveries
                    .Find(_ => true)
                    .ToListAsync(ct);

                return Result<List<Delivery>>.Success(items);
            }
        }
    }
}
