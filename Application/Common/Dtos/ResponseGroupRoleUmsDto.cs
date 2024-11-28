// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Application.Common.Dtos;

#pragma warning disable

public record ResponseGroupRoleUmsVm
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<ResponseGroupRoleUms> ResponseGroups { get; set; }
}

public record ResponseGroupRoleUms
{
    public string Name { get; set; }
    public List<string> ItemList { get; set; }
    public ResponseGroupRoleUmsDto Response { get; set; }
}

public record ResponseGroupRoleUmsDto
{
    public string Request { get; set; }
    public string Status { get; set; }
}
