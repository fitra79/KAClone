// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO.Compression;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Domain.Constants;

namespace Api.Handlers;

/// <summary>
/// CompressionHandler
/// </summary>
public static class CompressionHandler
{
    /// <summary>
    /// ApplyCompress
    /// </summary>
    /// <param name="services"></param>
    public static void ApplyCompress(IServiceCollection services)
    {
        services.AddResponseCompression(options =>
        {
            var mimeTypes = new[]
            {
                ConstantsHeader.JsonVndApi,
                ConstantsHeader.Pdf,
                ConstantsHeader.TextPlain,
                ConstantsHeader.ImageJpg,
                ConstantsHeader.Json,
                ConstantsHeader.OctetStream,
                ConstantsHeader.ProblemJson,
                ConstantsHeader.TextCsv,
                ConstantsHeader.ExcelXls,
                ConstantsHeader.ExcelXlsx
            };
            options.EnableForHttps = true;
            options.MimeTypes = mimeTypes;
            options.Providers.Add<GzipCompressionProvider>();
            options.Providers.Add<BrotliCompressionProvider>();
        });

        services.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Optimal;
        });
        services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Optimal;
        });
    }
}

/// <summary>
/// AddCompressionHandlerExtension
/// </summary>
public static class AddCompressionHandlerExtension
{
    /// <summary>
    /// AddCompressionHandler
    /// </summary>
    /// <param name="services"></param>
    public static void AddCompressionHandler(this IServiceCollection services)
    {
        CompressionHandler.ApplyCompress(services);
    }
}
