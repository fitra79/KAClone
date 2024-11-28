// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Domain.Constants;
using Application.Common.Exceptions;
using Polly;
using Polly.Contrib.WaitAndRetry;
using ValidationException = Application.Common.Exceptions.ValidationException;

namespace Application.Common.Behaviors;

/// <summary>
/// Applies a retry policy on the MediatR request.
/// Apply this attribute to the MediatR <see cref="IRequest"/> class (not on the handler).
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class RetryPolicyAttribute : Attribute
{
    private int _retryCount = 3;
    private int _sleepDuration = 200;

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets to enabling/disabling policy.
    /// Defaults to true.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the amount of times to retry the execution.
    /// Each retry the sleep duration is incremented by <see cref="SleepDuration"/>.
    /// Defaults to 3 times.
    /// </summary>
    public int RetryCount
    {
        get => _retryCount;
        set
        {
            if (value < 1)
            {
                throw new ArgumentException("Retry count must be higher than 1.", nameof(value));
            }

            _retryCount = value;
        }
    }

    /// <summary>
    /// Gets or sets the sleep duration in milliseconds.
    /// Each retry the sleep duration gets incremented by this value.
    /// Defaults to 200 milliseconds.
    /// </summary>
    public int SleepDuration
    {
        get => _sleepDuration;
        set
        {
            if (value < 1)
            {
                throw new ArgumentException(
                    "Sleep duration must be higher than 1ms.",
                    nameof(value));
            }

            _sleepDuration = value;
        }
    }

    /// <summary>
    /// Gets or sets the handle type.
    /// </summary>
    public Type HandleType { get; set; }
}

/// <summary>
/// Wraps request handler execution of requests decorated with the <see cref="RetryPolicyAttribute"/>
/// inside a policy to handle transient failures and retry the execution.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="RetryPolicyBehavior{TRequest, TResponse}"/> class.
/// </remarks>
/// <param name="_logger"></param>
/// <param name="_environment"></param>
public class RetryPolicyBehavior<TRequest, TResponse>(
    ILogger<RetryPolicyBehavior<TRequest, TResponse>> _logger,
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

        var retryAttr = typeof(TRequest).GetCustomAttribute<RetryPolicyAttribute>();

        if (retryAttr == null || !retryAttr.Enabled)
        {
            return await next().ConfigureAwait(false);
        }

        var delay = Backoff.DecorrelatedJitterBackoffV2(
            medianFirstRetryDelay: TimeSpan.FromMilliseconds(retryAttr.SleepDuration),
            retryCount: retryAttr.RetryCount);

        var requestName = typeof(TRequest).Name;

        return await Policy<TResponse>
            .Handle<Exception>(ex =>
            {
                if (ex.GetType() == retryAttr?.HandleType)
                {
                    return true;
                }

                return ex switch
                {
                    OperationCanceledException
                    or ValidationException
                    or BadRequestException
                    or NotFoundException
                        => false,
                    _ => true,
                };
            })
            .WaitAndRetryAsync(
                delay,
                (_, _) =>
                {
                    _logger.LogInformation("Retrying the request {name}", requestName);
                    return Task.CompletedTask;
                })
            .ExecuteAsync(() => next())
            .ConfigureAwait(false);
    }
}
