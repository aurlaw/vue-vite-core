using CsvHelper.Configuration;
using VueViteCore.Business.Entities;

namespace VueViteCore.Models.Maps;

public sealed class SubmissionEntryMap : ClassMap<SubmissionEntry>
{
    public SubmissionEntryMap()
    {
        Map(m => m.Id).Index(0).Name("id");
        Map(m => m.Name).Index(1).Name("name");
        Map(m => m.ValueOne).Index(2).Name("v1");
        Map(m => m.ValueTwo).Index(3).Name("v2");
        Map(m => m.ValueThree).Index(4).Name("v3");
        Map(m => m.Created).Index(5).Name("created");
    }
}
