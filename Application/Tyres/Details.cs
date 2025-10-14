using Application.Core;
using Domain;
using MediatR;
using MongoDB.Driver;
using Persistence;

namespace Application.Tyres
{
    public class Details
    {
        public class Query : IRequest<Result<Tyre>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Tyre>>
        {
            private readonly MongoDbContext _context;

            public Handler(MongoDbContext context)
            {
                _context = context;
            }

            public async Task<Result<Tyre>> Handle(Query request, CancellationToken cancellationToken)
            {
                var tyre = await _context.Tyres
                    .Find(t => t.Code == request.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (tyre == null)
                    return Result<Tyre>.Failure("Tyre not found");

                return Result<Tyre>.Success(tyre);
            }
        }
    }
}
