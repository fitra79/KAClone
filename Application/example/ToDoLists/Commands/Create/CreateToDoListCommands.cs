using Application.Common.Dtos;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ToDoLists.Commands.Create
{
    public class CreateToDoListCommand : ToDoListDto, IRequest<Unit>
    {

    }

    public class CreateToDoListCommandHandler : IRequestHandler<CreateToDoListCommand, Unit>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;
        public CreateToDoListCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;

        }
        public async Task<Unit> Handle(CreateToDoListCommand request, CancellationToken cancellationToken)
        {
            await context.ExecuteResiliencyAsync(() => HandleProcess(request, cancellationToken)).ConfigureAwait(false);
            return Unit.Value;
        }

        private async Task HandleProcess(CreateToDoListCommand request, CancellationToken cancellationToken)
        {

            var existingData = await context.Persons
            .AsNoTracking()
            .AnyAsync(x => x.Id == request.PersonId, cancellationToken)
            .ConfigureAwait(false);

            if (!existingData)
                throw new BadRequestException($"User with id : {request.PersonId} does not exist");

            var todolist = mapper.Map<ToDoListDto, ToDoList>(request);

            context.ToDoLists.Add(todolist);

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public class CreateToDoListCommandValidator : AbstractValidator<CreateToDoListCommand>
    {
        public CreateToDoListCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.PersonId).NotEmpty().WithMessage("{PropertyName} is required");
        }

    }

}