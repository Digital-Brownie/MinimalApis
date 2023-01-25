using JetBrains.Annotations;
using MinimalApis.Models;

namespace MinimalApis.Endpoints;

[UsedImplicitly]
public class CustomerEndpointDefinition : CrudEndpointDefinition<Customer, Guid>, IEndpointDefinition
{
    private const string BaseRouteField = "customers";
    protected override string BaseRoute => BaseRouteField;

    public void DefineEndpoints(WebApplication app)
    {
        MapCrudEndpoints(app);
    }
}
