using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Tyres
{
    public class Details
    {
        public class Query : IRequest<Result<Tyre>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Tyre>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Tyre>> Handle(Query request, CancellationToken cancellationToken)
            {
                var tyre = await _context.Tyres.FindAsync(request.Id);
                return Result<Tyre>.Success(tyre);
            }
        }
    }
}