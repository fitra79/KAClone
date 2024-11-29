using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Vms;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Constants;
using JsonApiSerializer.JsonApi;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Persons.Queries
{
    public class GetPersonQueries : QueryModel, IRequest<DocumentRootJson<List<PersonVm>>>
    {

    }

    public class GetPersonHandlers : IRequestHandler<GetPersonQueries, DocumentRootJson<List<PersonVm>>>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;
        public GetPersonHandlers(IApplicationDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;

        }
        public async Task<DocumentRootJson<List<PersonVm>>> Handle(GetPersonQueries request, CancellationToken cancellationToken)
        {
            var perosonQuery = await context.Persons
                .AsNoTracking()
                .AsSplitQuery()
                .ProjectTo<PersonVm>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return JsonApiExtensions.ToJsonApi(perosonQuery);
        }
    }
}