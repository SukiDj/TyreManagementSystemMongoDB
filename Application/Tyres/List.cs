using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using MongoDB.Driver;
using Persistence;

namespace Application.Tyres
{
    public class List
    {
        public class Query : IRequest<Result<List<TyreDto>>> { }

        public class Handler : IRequestHandler<Query, Result<List<TyreDto>>>
        {
            private readonly MongoDbContext _context;
            private readonly IMapper _mapper;

            public Handler(MongoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<List<TyreDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var tyres = await _context.Tyres.Find(_ => true).ToListAsync(cancellationToken);
                var tyresToReturn = _mapper.Map<List<TyreDto>>(tyres);

                return Result<List<TyreDto>>.Success(tyresToReturn);
            }
        }
    }
}
