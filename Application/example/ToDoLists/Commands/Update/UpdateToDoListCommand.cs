using Application.Common.Dtos;
using Application.Common.Interfaces;
using Application.Common.Models;
using Ardalis.GuardClauses;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ToDoLists.Commands.Update
{
    public class UpdateToDoListCommand : UpdateToDoListDto, IRequest<Unit>
    {

    }

    public class UpdateToDoListCommandHandler : IRequestHandler<UpdateToDoListCommand, Unit>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;
        public UpdateToDoListCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;

        }
        public async Task<Unit> Handle(UpdateToDoListCommand request, CancellationToken cancellationToken)
        {
            await context.ExecuteResiliencyAsync(() => HandleProcess(request, cancellationToken)).ConfigureAwait(false);
            return Unit.Value;
        }

        private async Task HandleProcess(UpdateToDoListCommand request, CancellationToken cancellationToken)
        {

            var id = new Guid(request.Id);

            var entity = await context.ToDoLists
                .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken)
                .ConfigureAwait(false);

            if (entity is null)
                throw new NotFoundException(request.Id, nameof(entity));

            mapper.Map(request, entity);

            context.ToDoLists.Update(entity);

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public class UpdateToDoListCommandValidator : AbstractValidator<UpdateToDoListCommand>
    {
        public UpdateToDoListCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(MustValidGuid)
                .WithMessage("{PropertyName} format invalid as Guid");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(20);
        }

        private static bool MustValidGuid(string arg)
        {
            return CustomValidator.IsValidGuid(arg);
        }
    }
}