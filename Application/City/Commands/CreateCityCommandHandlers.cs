using Application.Common.Dtos;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Cities.Commands.Create
{
    public class CreateCityCommand : CityDto, IRequest<Unit>
    {
    }

    public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateCityCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(CreateCityCommand request, CancellationToken cancellationToken)
        {
            await _context.ExecuteResiliencyAsync(() => HandleProcess(request, cancellationToken))
                          .ConfigureAwait(false);
            return Unit.Value;
        }

        private async Task HandleProcess(CreateCityCommand request, CancellationToken cancellationToken)
        {
            // Example of validation: Check if a city with the same name exists
            var existingCity = await _context.Cities
                                             .AsNoTracking()
                                             .AnyAsync(c => c.Name == request.Name, cancellationToken)
                                             .ConfigureAwait(false);

            if (existingCity)
                throw new BadRequestException($"A city with the name '{request.Name}' already exists.");

            var city = _mapper.Map<CityDto, City>(request);
            _context.Cities.Add(city);

            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
    {
        public CreateCityCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
