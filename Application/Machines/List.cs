using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Machines
{
    public class List
    {
        public class Query : IRequest<Result<List<MachineDto>>> {}

        public class Handler : IRequestHandler<Query, Result<List<MachineDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Result<List<MachineDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var machines = await _context.Machines.ToListAsync(cancellationToken);
                var machinesToReturn = _mapper.Map<List<MachineDto>>(machines);

                return Result<List<MachineDto>>.Success(machinesToReturn);
            }
        }
    }
}