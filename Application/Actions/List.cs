using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using MongoDB.Driver;
using Persistence;

namespace Application.Actions
{
    public class List
    {
        public class Query : IRequest<Result<List<ActionLog>>> { }

        public class Handler : IRequestHandler<Query, Result<List<ActionLog>>>
        {
            private readonly MongoDbContext _context;
            private readonly IMapper _mapper;

            public Handler(MongoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<List<ActionLog>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var actions = await _context.ActionLogs.Find(_ => true).ToListAsync(cancellationToken);
                var actionsToReturn = _mapper.Map<List<ActionLog>>(actions);

                return Result<List<ActionLog>>.Success(actionsToReturn);
            }
        }
    }
}
