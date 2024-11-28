// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AutoMapper;

namespace Application.Common.Mappings;

/// <summary>
/// IMapFrom
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IMapFrom<T>
{
    /// <summary>
    /// Mapping
    /// </summary>
    /// <param name="profile"></param>
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
}
