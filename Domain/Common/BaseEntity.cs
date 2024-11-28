// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common;

/// <summary>
/// BaseEntity
/// </summary>
public record BaseEntity
{
    /// <summary>
    /// Gets or sets id
    /// </summary>
    /// <value></value>
    public Guid Id { get; set; }

    private readonly List<BaseEvent> _domainEvents = [];

    /// <summary>
    /// Gets DomainEvents
    /// </summary>
    /// <value></value>
    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// GetUserListAsync
    /// </summary>
    /// <param name="domainEvent"></param>
    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// RemoveDomainEvent
    /// </summary>
    /// <param name="domainEvent"></param>
    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    /// <summary>
    /// ClearDomainEvents
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
