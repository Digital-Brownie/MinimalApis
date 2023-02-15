using System.Linq.Expressions;
using JetBrains.Annotations;
using MinimalApis.Models;
using MinimalApis.Queries;
using MinimalApis.Repos;
using MinimalApis.Queries;

namespace MinimalApis.Endpoints;

[UsedImplicitly]
public class CustomerEndpointDefinition : CrudEndpointDefinition<Customer, Guid, CustomerQuery>, IEndpointDefinition
{
    private const string BaseRouteField = "customers";
    protected override string BaseRoute => BaseRouteField;
    protected override List<Expression<Func<Customer, bool>>> QueryParams { get; } = new();

    public void DefineEndpoints(WebApplication app)
    {
        MapCrudEndpoints(app);
    }

    protected override IEnumerable<Customer> Query(IRepository<Customer, Guid> repository, CustomerQuery query)
    {
        var results = base.Query(repository, query);

        if (query.Username is not null)
        {
            results = results.Where(c => c.Username == query.Username);
        }

        if (query.UsernameLike is not null)
        {
            results = results.Where(c => c.Username.Contains(query.UsernameLike));
        }

        if (query.AgeGreaterThan is not null)
        {
            results = results
                .Where(
                    c => DateOnly.FromDateTime(DateTime.Now).Year - c.DateOfBirth.Year > query.AgeGreaterThan
                    );
        }

        return results;
    }
}
