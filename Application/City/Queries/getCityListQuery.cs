using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Vms;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using JsonApiSerializer.JsonApi;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Cities.Queries
{
    public class GetCityListQuery : QueryModel, IRequest<DocumentRootJson<List<CityVm>>>
    {

    }

    public class GetCityListHandler : IRequestHandler<GetCityListQuery, DocumentRootJson<List<CityVm>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCityListHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DocumentRootJson<List<CityVm>>> Handle(GetCityListQuery request, CancellationToken cancellationToken)
        {
            var cityQuery = await _context.Cities
                .AsNoTracking()
                .AsSplitQuery()
                .ProjectTo<CityVm>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return JsonApiExtensions.ToJsonApi(cityQuery);
        }
    }
}
