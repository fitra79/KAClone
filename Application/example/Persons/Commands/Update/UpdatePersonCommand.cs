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

namespace Application.Persons.Commands.Update
{
    public class UpdatePersonCommand : UpdatePersonDto, IRequest<Unit>
    {

    }

    public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, Unit>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;
        public UpdatePersonCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;

        }
        public async Task<Unit> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            await context.ExecuteResiliencyAsync(() => HandleProcess(request, cancellationToken)).ConfigureAwait(false);
            return Unit.Value;
        }

        private async Task HandleProcess(UpdatePersonCommand request, CancellationToken cancellationToken)
        {

            var id = new Guid(request.Id);

            var entity = await context.Persons
                .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken)
                .ConfigureAwait(false);

            if (entity is null)
                throw new NotFoundException(request.Id, nameof(entity));

            mapper.Map(request, entity);

            context.Persons.Update(entity);

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public class UpdatePersonCommandValidator : AbstractValidator<UpdatePersonCommand>
    {
        public UpdatePersonCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(MustValidGuid)
                .WithMessage("{PropertyName} format invalid as Guid");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(20);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(255);

        }

        private static bool MustValidGuid(string arg)
        {
            return CustomValidator.IsValidGuid(arg);
        }
    }
}