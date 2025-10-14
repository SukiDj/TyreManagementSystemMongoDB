using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using MongoDB.Driver;
using Persistence;

namespace Application.Clients
{
    public class List
    {
        public class Query : IRequest<Result<List<ClientDto>>> { }

        public class Handler : IRequestHandler<Query, Result<List<ClientDto>>>
        {
            private readonly MongoDbContext _context;
            private readonly IMapper _mapper;

            public Handler(MongoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<List<ClientDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var clients = await _context.Clients.Find(_ => true).ToListAsync(cancellationToken);
                var clientsToReturn = _mapper.Map<List<ClientDto>>(clients);

                return Result<List<ClientDto>>.Success(clientsToReturn);
            }
        }
    }
}
