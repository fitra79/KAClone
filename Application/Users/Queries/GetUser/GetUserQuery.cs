using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Vms;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.GetUser;

public class GetUserQuery : QueryModel, IRequest<DocumentRootJson<List<UserVm>>>
{
}

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, DocumentRootJson<List<UserVm>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DocumentRootJson<List<UserVm>>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var userQuery = await _context.Users
            .AsNoTracking()
            .AsSplitQuery()
            .ProjectTo<UserVm>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return JsonApiExtensions.ToJsonApi(userQuery);
    }
}