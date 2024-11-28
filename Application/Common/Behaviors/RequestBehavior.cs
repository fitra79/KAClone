// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MediatR;
using Microsoft.Extensions.Logging;
using Application.Common.Exceptions;

namespace Application.Common.Behaviors;

/// <summary>
/// RequestBehavior
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="RequestBehavior{TRequest, TResponse}"/> class.
/// </remarks>
/// <param name="_logger"></param>
public class RequestBehavior<TRequest, TResponse>(
    ILogger<RequestBehavior<TRequest, TResponse>> _logger
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
        var requestType = typeof(TRequest).Name;

        var response = await next().ConfigureAwait(false);

        if (requestType.EndsWith("Command"))
        {
            _logger.LogDebug("Command Request: {request}", request);
        }
        else if (requestType.EndsWith("Query"))
        {
            _logger.LogDebug("Query Request: {request}", request);
            _logger.LogDebug("Query Response: {response}", response);
        }
        else
        {
            throw new ThrowException("The request is not the Command or Query type");
        }

        return response;
    }
}
