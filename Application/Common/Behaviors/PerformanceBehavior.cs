// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using Domain.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;

namespace Application.Common.Behaviors;

/// <summary>
/// PerformanceBehavior
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="PerformanceBehavior{TRequest, TResponse}"/> class.
/// </remarks>
/// <param name="_logger"></param>
/// <param name="_userAuthorizationService"></param>
/// <param name="_appSetting"></param>
public class PerformanceBehavior<TRequest, TResponse>(
    ILogger<TRequest> _logger,
    IUserAuthorizationService _userAuthorizationService,
    AppSetting _appSetting) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer = new Stopwatch();

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResponse> Handle(
        TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next().ConfigureAwait(false);

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds <= _appSetting.RequestPerformanceInMs)
        {
            return response;
        }

        var requestName = typeof(TRequest).Name;
        var user = _userAuthorizationService.GetAuthorizedUser();
        var userName = user.UserName ?? ConstantsSystem.Name;

        _logger.LogWarning(
            "{Namespace} Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserName} {@Request}",
            _appSetting.App.Namespace,
            requestName,
            elapsedMilliseconds,
            userName,
            request);

        return response;
    }
}
