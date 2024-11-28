// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Domain.Constants;
using Application.Common.Exceptions;
using Application.Common.Models;
using Polly;
using Polly.Bulkhead;
using Polly.CircuitBreaker;
using Polly.RateLimit;
using Polly.Timeout;
using ValidationException = Application.Common.Exceptions.ValidationException;

namespace Application.Common.Behaviors;

/// <summary>
/// Applies a circuit breaker policy on the MediatR request.
/// Apply this attribute to the MediatR <see cref="IRequest"/> class (not on the handler).
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class CircuitBreakerPolicyAttribute : Attribute
{
    private int _exceptionAllowed = 3;
    private int _durationOfBreak = 30;

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets to enabling/disabling policy.
    /// Defaults to true.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the amount of times to exception allowed the execution.
    /// Defaults to 3 times.
    /// </summary>
    public int ExceptionAllowed
    {
        get => _exceptionAllowed;
        set
        {
            if (value < 1)
            {
                throw new ArgumentException(
                    "ExceptionAllowed must be higher than 1.",
                    nameof(value));
            }

            _exceptionAllowed = value;
        }
    }

    /// <summary>
    /// Gets or sets the duration of break in seconds.
    /// Defaults to 30 seconds.
    /// </summary>
    public int DurationOfBreak
    {
        get => _durationOfBreak;
        set
        {
            if (value < 1)
            {
                throw new ArgumentException(
                    "Duration of break must be higher than 1 second.",
                    nameof(value));
            }

            _durationOfBreak = value;
        }
    }

    /// <summary>
    /// Gets or sets the handle type.
    /// </summary>
    public Type HandleType { get; set; }
}

/// <summary>
/// Wraps request handler execution of requests decorated with the <see cref="CircuitBreakerPolicyAttribute"/>
/// inside a policy to handle transient failures and circuit breaker the execution.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="CircuitBreakerPolicyBehavior{TRequest, TResponse}"/> class.
/// </remarks>
/// <param name="_logger"></param>
/// <param name="appSetting"></param>
/// <param name="_environment"></param>
public class CircuitBreakerPolicyBehavior<TRequest, TResponse>(
    ILogger<CircuitBreakerPolicyBehavior<TRequest, TResponse>> _logger,
    AppSetting appSetting,
    IWebHostEnvironment _environment
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly CircuitBreaker _circuitBreakerPolicy = appSetting
        .ResiliencyPolicy
        .CircuitBreaker;

    private readonly string _requestName = typeof(TRequest).Name;

    private AsyncCircuitBreakerPolicy<TResponse> _circuitBreaker;

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

        var circuitBreakerAttr = typeof(TRequest).GetCustomAttribute<CircuitBreakerPolicyAttribute>();

        if (circuitBreakerAttr is { Enabled: false } || (circuitBreakerAttr == null && !_circuitBreakerPolicy.Enabled))
        {
            return await next().ConfigureAwait(false);
        }

        _circuitBreaker ??= Policy<TResponse>
            .Handle<Exception>(ex =>
            {
                if (ex.GetType() == circuitBreakerAttr?.HandleType)
                {
                    return true;
                }

                return ex switch
                {
                    BulkheadRejectedException
                    or RateLimitRejectedException
                    or TimeoutRejectedException
                    or OperationCanceledException
                    or ValidationException
                    or BadRequestException
                    or NotFoundException
                        => false,
                    _ => true,
                };
            })
            .CircuitBreakerAsync(
                circuitBreakerAttr?.ExceptionAllowed ?? _circuitBreakerPolicy.ExceptionAllowed,
                TimeSpan.FromSeconds(
                    circuitBreakerAttr?.DurationOfBreak ?? _circuitBreakerPolicy.DurationOfBreak),
                OnBreak,
                OnReset);

        return await _circuitBreaker.ExecuteAsync(() => next()).ConfigureAwait(false);
    }

    private void OnBreak(DelegateResult<TResponse> _, TimeSpan timeSpan)
    {
        _logger.LogInformation(
            "Circuit breaker open for request {name}, reset in {time} seconds",
            _requestName,
            timeSpan.Seconds);
    }

    private void OnReset()
    {
        _logger.LogInformation("Resetting Circuit breaker for request {name}", _requestName);
    }
}
