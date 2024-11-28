// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Application.Common.Dtos;

#pragma warning disable
public record ResponsePermissionUmsVm
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<ResponsePermissionUmsDto> ResponsePermissionDtos { get; set; }
}

public record ResponsePermissionUmsDto
{
    public string Name { get; set; }
    public string Status { get; set; }
}
