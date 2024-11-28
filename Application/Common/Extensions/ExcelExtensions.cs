// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Data;
using System.IO;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Domain.Constants;

namespace Application.Common.Extensions;

/// <summary>
/// Excel Extension
/// </summary>
public static class ExcelExtensions
{
    /// <summary>
    /// Read Xlsx
    /// </summary>
    /// <param name="file"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<DataTable> FromXlsx(this IFormFile file, CancellationToken cancellationToken)
    {
        var stream = new MemoryStream();
        var dataTable = new DataTable();

        await using (stream.ConfigureAwait(false))
        {
            await file.CopyToAsync(stream, cancellationToken).ConfigureAwait(false);

            using var wb = new XLWorkbook(stream);
            var firstRow = true;

            foreach (var row in wb.Worksheet(1).Rows())
            {
                if (row.IsEmpty())
                {
                    row.Cells().Clear();
                }

                if (!row.Cells().Any())
                {
                    break;
                }

                if (firstRow)
                {
                    foreach (var cell in row.Cells())
                    {
                        dataTable.Columns.Add(cell.Value.ToString());
                    }

                    firstRow = false;
                }
                else
                {
                    dataTable.Rows.Add();
                    var i = 0;

                    for (var j = 1; j <= dataTable.Columns.Count; j++)
                    {
                        var activeCell = $"{(char)(ConstantsPagination.CustomRuleAsciiAlphabet + j)}{dataTable.Rows.Count + 1}";

                        // var combinePattern = ConstantsRegex.Pattern.Replace(
                        //     "{pattern}",
                        //     ConstantsRegex.Char + ConstantsRegex.Numeric + " " + ConstantsRegex.Symbol);
                        // var pattern = new Regex(combinePattern);
                        //
                        // if (!string.IsNullOrWhiteSpace(row.Cells(activeCell).Select(x => x).FirstOrDefault()?.Value.ToString()))
                        // {
                        //     var isMatch = pattern.IsMatch(
                        //         row.Cells(activeCell).Select(x => x).FirstOrDefault()?.Value.ToString() ?? string.Empty);
                        //
                        //     if (!isMatch)
                        //     {
                        //         throw new BadRequestException($"Only character [{ConstantsRegex.Char}], numeric [{ConstantsRegex.Numeric}], and symbol [{ConstantsRegex.Symbol}] can be accepted in cell {activeCell}");
                        //     }
                        // }

                        dataTable.Rows[^1][i] = row.Cells(activeCell).Select(x => x).FirstOrDefault()?.Value.ToString() ?? string.Empty;

                        i++;
                    }
                }
            }

            stream.Close();
        }

        return dataTable;
    }
}
