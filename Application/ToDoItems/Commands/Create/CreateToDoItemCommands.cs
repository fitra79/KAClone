using Application.Common.Dtos;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ToDoItems.Commands.Create
{
    public class CreateToDoItemCommand : ToDoItemDto, IRequest<Unit>
    {

    }

    public class CreateToDoItemCommandHandler : IRequestHandler<CreateToDoItemCommand, Unit>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;
        public CreateToDoItemCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;

        }
        public async Task<Unit> Handle(CreateToDoItemCommand request, CancellationToken cancellationToken)
        {
            await context.ExecuteResiliencyAsync(() => HandleProcess(request, cancellationToken)).ConfigureAwait(false);
            return Unit.Value;
        }

        private async Task HandleProcess(CreateToDoItemCommand request, CancellationToken cancellationToken)
        {

            var existingData = await context.ToDoLists
            .AsNoTracking()
            .AnyAsync(x => x.Id == request.TodoListId, cancellationToken)
            .ConfigureAwait(false);

            if (!existingData)
                throw new BadRequestException($"To do list with id : {request.TodoListId} does not exist");

            var todoitem = mapper.Map<ToDoItemDto, ToDoItem>(request);

            context.ToDoItems.Add(todoitem);

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public class CreateToDoItemCommandValidator : AbstractValidator<CreateToDoItemCommand>
    {
        public CreateToDoItemCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.TodoListId).NotEmpty().WithMessage("{PropertyName} is required");
        }

    }

}