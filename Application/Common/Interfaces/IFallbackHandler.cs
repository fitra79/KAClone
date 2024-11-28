// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MediatR;

namespace Application.Common.Interfaces;

/// <summary>
/// Interface of fallback policy implementation.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IFallbackHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// HandleFallback
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<TResponse> HandleFallback(TRequest request, CancellationToken cancellationToken);
}
