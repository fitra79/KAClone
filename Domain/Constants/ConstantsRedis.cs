// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Domain.Constants;

/// <summary>
/// ConstantsRedis
/// </summary>
public abstract class ConstantsRedis
{
    /// <summary>
    /// RedisSubKeyConsumeMessage
    /// </summary>
    public const string SubKeyConsumeMessage = "ConsumeMessage";

    /// <summary>
    /// RedisSubKeyProduceMessage
    /// </summary>
    public const string SubKeyProduceMessage = "ProduceMessage";

    /// <summary>
    /// RedisSubKeyHttpRequest
    /// </summary>
    public const string SubKeyHttpRequest = "HttpRequest";
}
