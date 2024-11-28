// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.Serialization;

namespace Application.Common.Exceptions;

/// <summary>
/// ThrowException
/// </summary>
[Serializable]
public class ThrowException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ThrowException"/> class.
    /// </summary>
    /// <param name="message"></param>
    public ThrowException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ThrowException"/> class.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected ThrowException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
