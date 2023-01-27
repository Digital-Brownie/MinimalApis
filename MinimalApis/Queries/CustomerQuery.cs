using System.Reflection;
using JetBrains.Annotations;

namespace MinimalApis.Queries;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public record CustomerQuery(int Count, int Page) : QueryBase(Count, Page)
{
    public string? Username { get; init; }
    public string? UsernameLike { get; init; }
    public string? Email { get; init; }
    public int? Age { get; init; }
    public int? AgeGreaterThan { get; init; }
    public int? AgeLessThan { get; init; }

    public static ValueTask<CustomerQuery> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        var _ = TryParse(context.Request.Query, out var baseResult);
        var (count, page) = baseResult;

        var query = FromQuery(context.Request.Query)
            .ToDictionary(p => p.Key, p => p.Value);

        var result = new CustomerQuery(count, page);
        result = result with
        {
            Username = query.GetValueOrDefault(nameof(Username)),
            UsernameLike = query.GetValueOrDefault(nameof(UsernameLike)),
            Email = query.GetValueOrDefault(nameof(Email)),
            Age = int.TryParse(query.GetValueOrDefault(nameof(Age)), out var parsedAge) ? parsedAge : null,
            AgeGreaterThan = int.TryParse(query.GetValueOrDefault(nameof(AgeGreaterThan)), out var parsedAgeGreaterThan)
                ? parsedAgeGreaterThan
                : null,
            AgeLessThan = int.TryParse(query.GetValueOrDefault(nameof(AgeLessThan)), out var parsedLessThan) ? parsedLessThan : null,
        };

        return ValueTask.FromResult(result);
    }
}
