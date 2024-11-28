// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Domain.Constants;
using Application.Common.Interfaces;
using Polly;

namespace Application.Common.Behaviors;

/// <summary>
/// Wraps request handler execution of requests
/// inside a policy to handle transient fallback the execution.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="FallbackBehavior{TRequest, TResponse}"/> class.
/// </remarks>
/// <param name="_fallbackHandlers"></param>
/// <param name="_logger"></param>
/// <param name="_environment"></param>
public class FallbackBehavior<TRequest, TResponse>(
    IEnumerable<IFallbackHandler<TRequest, TResponse>> _fallbackHandlers,
    ILogger<FallbackBehavior<TRequest, TResponse>> _logger,
    IWebHostEnvironment _environment
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_environment.EnvironmentName.Equals(ConstantsEnvironment.NameTest))
        {
            return await next().ConfigureAwait(false);
        }

        var fallbackHandler = _fallbackHandlers.FirstOrDefault();

        if (fallbackHandler == null)
        {
            return await next().ConfigureAwait(false);
        }

        var requestName = typeof(TRequest).Name;

        return await Policy<TResponse>
            .Handle<Exception>()
            .FallbackAsync(
                async (cancellationToken) =>
                {
                    _logger.LogInformation("Falling back response for request {name}", requestName);

                    return await fallbackHandler
                        .HandleFallback(request, cancellationToken)
                        .ConfigureAwait(false);
                })
            .ExecuteAsync(() => next())
            .ConfigureAwait(false);
    }
}
