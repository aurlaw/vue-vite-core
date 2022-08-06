namespace VueViteCore.Models;


public record ExportResult(Stream Stream, string ContentType, string FileName);

public enum ExportType
{
    Csv,
    Excel
}