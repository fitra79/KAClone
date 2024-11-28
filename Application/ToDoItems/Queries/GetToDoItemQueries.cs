using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Vms;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ToDoItems.Queries
{
    public class GetToDoItemQueries : QueryModel, IRequest<DocumentRootJson<List<ToDoItemVm>>>
    {

    }

    public class GetToDoItemHandlers : IRequestHandler<GetToDoItemQueries, DocumentRootJson<List<ToDoItemVm>>>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;
        public GetToDoItemHandlers(IApplicationDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;

        }
        public async Task<DocumentRootJson<List<ToDoItemVm>>> Handle(GetToDoItemQueries request, CancellationToken cancellationToken)
        {
            var todoitemQuery = await context.ToDoItems
                .AsNoTracking()
                .AsSplitQuery()
                .ProjectTo<ToDoItemVm>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return JsonApiExtensions.ToJsonApi(todoitemQuery);
        }
    }
}