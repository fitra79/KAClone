// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Application.Common.Interfaces;

/// <summary>
/// IProducerService
/// </summary>
public interface IProducerService
{
    /// <summary>
    /// SendAsync
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    Task<bool> SendAsync(string topic, string message);
}
