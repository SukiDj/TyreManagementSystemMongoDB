using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Actions
{
    public class List
    {
        public class Query : IRequest<Result<List<ActionLog>>> {}

        public class Handler : IRequestHandler<Query, Result<List<ActionLog>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Result<List<ActionLog>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var actions = await _context.ActionLogs.ToListAsync(cancellationToken);
                var actionsToReturn = _mapper.Map<List<ActionLog>>(actions);

                return Result<List<ActionLog>>.Success(actionsToReturn);
            }
        }
    }
}