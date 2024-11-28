// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.Serialization;

namespace Application.Common.Exceptions;

/// <summary>
/// BadRequestException
/// </summary>
[Serializable]
public class BadRequestException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class.
    /// </summary>
    /// <param name="message"></param>
    public BadRequestException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected BadRequestException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
