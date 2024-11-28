// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.Serialization;
using FluentValidation.Results;

namespace Application.Common.Exceptions;

/// <summary>
/// ValidationException
/// </summary>
[Serializable]
public class ValidationException : Exception
{
    /// <summary>
    /// Gets errors
    /// </summary>
    /// <value></value>
    public IDictionary<string, string[]> Errors { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class.
    /// </summary>
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class.
    /// </summary>
    /// <param name="failures"></param>
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.Distinct().ToArray());
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected ValidationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
