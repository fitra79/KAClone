using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Dtos;
using Application.Common.Interfaces;
using Application.Common.Models;
using Ardalis.GuardClauses;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ToDoItems.Commands.Update
{
    public class UpdateToDoItemCommand : UpdateToDoItemDto, IRequest<Unit>
    {

    }

    public class UpdateToDoItemCommandHandler : IRequestHandler<UpdateToDoItemCommand, Unit>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;
        public UpdateToDoItemCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;

        }
        public async Task<Unit> Handle(UpdateToDoItemCommand request, CancellationToken cancellationToken)
        {
            await context.ExecuteResiliencyAsync(() => HandleProcess(request, cancellationToken)).ConfigureAwait(false);
            return Unit.Value;
        }

        private async Task HandleProcess(UpdateToDoItemCommand request, CancellationToken cancellationToken)
        {

            var id = new Guid(request.Id);

            var entity = await context.ToDoItems
                .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken)
                .ConfigureAwait(false);

            if (entity is null)
                throw new NotFoundException(request.Id, nameof(entity));

            mapper.Map(request, entity);

            context.ToDoItems.Update(entity);

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public class UpdateToDoItemCommandValidator : AbstractValidator<UpdateToDoItemCommand>
    {
        public UpdateToDoItemCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(MustValidGuid)
                .WithMessage("{PropertyName} format invalid as Guid");

            // RuleFor(x => x.Title)
            //     .NotEmpty().WithMessage("{PropertyName} is required")
            //     .MaximumLength(20);
        }

        private static bool MustValidGuid(string arg)
        {
            return CustomValidator.IsValidGuid(arg);
        }
    }
}