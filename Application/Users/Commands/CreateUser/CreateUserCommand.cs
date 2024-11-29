using Application.Common.Dtos;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Users.Commands.CreateUser;

public class CreateUserCommand : UserDto, IRequest<Unit>
{
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await _context.ExecuteResiliencyAsync(() => HandleProcess(request, cancellationToken)).ConfigureAwait(false);
        return Unit.Value;
    }

    private async Task HandleProcess(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<User>(request);
        
        _context.Users.Add(entity);

        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}