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

namespace Application.ToDoLists.Commands.Delete;
public class DeleteToDoListCommand : IRequest<Unit>
{
   
    [BindNever]
    public string Id { get; set; }
}

/// <summary>
/// DeleteDepartmentCommandHandler
/// </summary>
public class DeleteToDoListCommandHandler : IRequestHandler<DeleteToDoListCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    public DeleteToDoListCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteToDoListCommand request, CancellationToken cancellationToken)
    {
        await _context.ExecuteResiliencyAsync(() => HandleProcess(request, cancellationToken)).ConfigureAwait(false);

        return Unit.Value;
    }

    private async Task HandleProcess(DeleteToDoListCommand request, CancellationToken cancellationToken)
    {

        var id = new Guid(request.Id);

        var entity = await _context.ToDoLists
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken)
            .ConfigureAwait(false);

        if (entity is null)
            throw new NotFoundException(request.Id, nameof(entity));

        entity.IsActive = false;
        entity.IsDelete = true;

        _context.ToDoLists.Update(entity);

        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}

public class DeleteToDoListValidator : AbstractValidator<DeleteToDoListCommand>
{
    
    public DeleteToDoListValidator()
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
