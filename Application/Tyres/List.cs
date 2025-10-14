using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Tyres
{
    public class List
    {
        public class Query : IRequest<Result<List<TyreDto>>> {}

        public class Handler : IRequestHandler<Query, Result<List<TyreDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Result<List<TyreDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var tyres = await _context.Tyres.ToListAsync(cancellationToken);
                var tyresToReturn = _mapper.Map<List<TyreDto>>(tyres);

                return Result<List<TyreDto>>.Success(tyresToReturn);
            }
        }
    }
}