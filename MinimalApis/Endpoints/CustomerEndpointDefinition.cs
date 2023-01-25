using JetBrains.Annotations;
using MinimalApis.Models;
using MinimalApis.Repos;

namespace MinimalApis.Endpoints;

[UsedImplicitly]
public class CustomerEndpointDefinition : CrudEndpointDefinition<Customer, Guid>, IEndpointDefinition
{
    private const string BaseRouteField = "customers";
    protected override string BaseRoute => BaseRouteField;

    public void DefineServices(IServiceCollection services)
    {
        services.AddSingleton<IRepository<Customer, Guid>, CustomerRepository>();
    }

    public void DefineEndpoints(WebApplication app)
    {
        MapCrudEndpoints(app);
    }
}
