// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Domain.Constants;

/// <summary>
/// ConstantsHealthCheck
/// </summary>
public abstract class ConstantsHealthCheck
{
    /// <summary>
    /// DefaultHealthCheckQuery
    /// </summary>
    public const string Query = "SELECT \"Id\" From \"Changelogs\" Limit 1";

    /// <summary>
    /// DefaultHealthCheckTimeoutInSeconds
    /// </summary>
    public const int TimeoutInSeconds = 60;

    /// <summary>
    /// DefaultHealthCheckCpuUsage
    /// </summary>
    public const string CpuUsage = "CpuUsage";

    /// <summary>
    /// DefaultHealthCheckMemoryUsage
    /// </summary>
    public const string MemoryUsage = "Memory";

    /// <summary>
    /// DefaultHealthCheckDatabaseName
    /// </summary>
    public const string DatabaseName = "DB";

    /// <summary>
    /// DefaultHealthCheckUmsName
    /// </summary>
    public const string UmsName = "UserManagementService";

    /// <summary>
    /// DefaultHealthCheckMLName
    /// </summary>
    public const string MLName = "MachineLearningService";

    /// <summary>
    /// DefaultHealthCheckGateWayName
    /// </summary>
    public const string GateWayName = "Gateway";

    /// <summary>
    /// DefaultHealthCheckKafkaName
    /// </summary>
    public const string KafkaName = "Kafka";

    /// <summary>
    /// DefaultHealthCheckEventHub
    /// </summary>
    public const string EventHub = "EventHub";

    /// <summary>
    /// DefaultHealthCheckStorageAccountEventHub
    /// </summary>
    public const string StorageAccountEventHub = "StorageAccountEventHub";

    /// <summary>
    /// DefaultHealthCheckEventHubName
    /// </summary>
    public const string EventHubName = "EventHub";

    /// <summary>
    /// DefaultHealthCheckRedisName
    /// </summary>
    public const string RedisName = "Redis";

    /// <summary>
    /// DefaultHealthCheckPercentageUsedDegraded
    /// </summary>
    public const byte PercentageUsedDegraded = 90;
}
