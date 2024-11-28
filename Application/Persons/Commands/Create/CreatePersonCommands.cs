using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Dtos;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Persons.Commands.Create
{
    public class CreatePersonCommand : PersonDto, IRequest<Unit>
    {

    }

    public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, Unit>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;
        public CreatePersonCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;

        }
        public async Task<Unit> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            await context.ExecuteResiliencyAsync(() => HandleProcess(request, cancellationToken)).ConfigureAwait(false);
            return Unit.Value;
        }

        private async Task HandleProcess(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            var person = mapper.Map<PersonDto, Person>(request);

            context.Persons.Add(person);

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
    {
        public CreatePersonCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }

}