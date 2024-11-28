// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Application.Common.Interfaces;

/// <summary>
/// IEmailRequestService
/// </summary>
public interface IEmailRequestService
{
    /// <summary>
    /// PosTask
    /// </summary>
    /// <param name="url"></param>
    /// <param name="data"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task PosTask(string url, string data, CancellationToken cancellationToken);
}
