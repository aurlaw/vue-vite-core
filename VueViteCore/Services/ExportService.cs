using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Excel;
using VueViteCore.Models;

namespace VueViteCore.Services;

public class ExportService
{
    
    public async Task<ExportResult> GetExportedStream<T,TMap>(IEnumerable<T> records, 
        ExportType exportType, 
        string filename,
        CancellationToken cancellationToken) where TMap : ClassMap
    {
        // create memory stream to write  data to
        var memoryStream = new MemoryStream();
        await using (var excelWriter = GetWriter(memoryStream, exportType))
        {
            excelWriter.Context.RegisterClassMap<TMap>();
            await excelWriter.WriteRecordsAsync(records, cancellationToken);
        }

        // set the position to return the file from
        memoryStream.Position = 0;
        var ext = exportType == ExportType.Excel ? ".xlsx" : ".csv";
        var mime = exportType == ExportType.Excel ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "text/csv";
        var f = $"{filename}_{DateTime.UtcNow:u}.{ext}";

        return new ExportResult(memoryStream, mime, f);
    }
    
    
    private CsvWriter GetWriter(Stream stream, ExportType exportType)
    {
        if (exportType == ExportType.Excel)
        {
            return new ExcelWriter(stream, CultureInfo.InvariantCulture, true);
        } 
        var writer = new StreamWriter(stream);
        return new CsvWriter(writer, CultureInfo.InvariantCulture, true);
    }
    
}