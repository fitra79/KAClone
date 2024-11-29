// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#region

using Ardalis.GuardClauses;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Application.Common.Models;

#endregion

namespace Application.Persons.Commands.Delete;
public class DeletePersonCommand : IRequest<Unit>
{
   
    [BindNever]
    public string Id { get; set; }
}

public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    public DeletePersonCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        await _context.ExecuteResiliencyAsync(() => HandleProcess(request, cancellationToken)).ConfigureAwait(false);

        return Unit.Value;
    }

    private async Task HandleProcess(DeletePersonCommand request, CancellationToken cancellationToken)
    {

        var id = new Guid(request.Id);

        var entity = await _context.Persons
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken)
            .ConfigureAwait(false);

        if (entity is null)
            throw new NotFoundException(request.Id, nameof(entity));

        entity.IsActive = false;
        entity.IsDelete = true;

        _context.Persons.Update(entity);

        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}

public class DeletePersonValidator : AbstractValidator<DeletePersonCommand>
{
    
    public DeletePersonValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Must(MustValidGuid)
            .WithMessage("{PropertyName} format invalid as Guid");
    }

    private static bool MustValidGuid(string arg)
    {
        return CustomValidator.IsValidGuid(arg);
    }
}
