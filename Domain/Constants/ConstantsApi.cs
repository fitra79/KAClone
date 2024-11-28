// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Domain.Constants;

/// <summary>
/// ConstantsApi
/// </summary>
public abstract class ConstantsApi
{
    /// <summary>
    /// ApiErrorDescription
    /// </summary>
    public readonly record struct ApiErrorDescription
    {
        /// <summary>
        /// BadRequest
        /// </summary>
        public const string BadRequest = "BadRequest";

        /// <summary>
        /// Unauthorized
        /// </summary>
        public const string Unauthorized = "Unauthorized";

        /// <summary>
        /// Forbidden
        /// </summary>
        public const string Forbidden = "Forbidden";

        /// <summary>
        /// InternalServerError
        /// </summary>
        public const string InternalServerError = "InternalServerError";
    }
}
