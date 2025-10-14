using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Clients
{
    public class List
    {
        public class Query : IRequest<Result<List<ClientDto>>> {}

        public class Handler : IRequestHandler<Query, Result<List<ClientDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Result<List<ClientDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var clients = await _context.Clients.ToListAsync(cancellationToken);
                var clientsToReturn = _mapper.Map<List<ClientDto>>(clients);

                return Result<List<ClientDto>>.Success(clientsToReturn);
            }
        }
    }
}