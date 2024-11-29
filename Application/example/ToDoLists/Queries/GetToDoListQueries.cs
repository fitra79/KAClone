using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Vms;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Constants;
using Domain.Entities;
using JsonApiSerializer.JsonApi;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ToDoLists.Queries
{
    public class GetToDoListQueries : QueryModel, IRequest<DocumentRootJson<List<ToDoListVm>>>
    {

    }

    public class GetToDoListHandlers : IRequestHandler<GetToDoListQueries, DocumentRootJson<List<ToDoListVm>>>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;
        public GetToDoListHandlers(IApplicationDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;

        }
        public async Task<DocumentRootJson<List<ToDoListVm>>> Handle(GetToDoListQueries request, CancellationToken cancellationToken)
        {
            var todolistQuery = await context.ToDoLists
                .AsNoTracking()
                .AsSplitQuery()
                .ProjectTo<ToDoListVm>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return JsonApiExtensions.ToJsonApi(todolistQuery);
        }
    }
}