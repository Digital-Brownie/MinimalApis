using Humanizer;

namespace MinimalApis.Queries;

public record QueryBase(int Count, int Page)
{
    protected static bool TryParse(IQueryCollection queryString, out QueryBase result)
    {
        var query = FromQuery(queryString)
            .ToDictionary(p => p.Key, p => p.Value);
        var count = 10;
        var page = 1;
        if (int.TryParse(query.GetValueOrDefault(nameof(Count)), out var parsedCount))
        {
            count = parsedCount;
        }

        if (int.TryParse(query.GetValueOrDefault(nameof(Page)), out var parsedPage))
        {
            page = parsedPage;
        }

        result = new(count, page);
        return true;
    }

    protected static IEnumerable<(string Key, string Value)> FromQuery(IQueryCollection query)
    {
        foreach (var param in query)
        {
            yield return (param.Key.Pascalize(), param.Value)!;
        }
    }
}
