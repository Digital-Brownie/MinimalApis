using MinimalApis.Endpoints;
using MinimalApis.Models;
using MinimalApis.Utils;

namespace MinimalApis.Repos;

[Injected<IRepository<Customer, Guid>>(ServiceLifetime.Singleton)]
public class CustomerRepository : RepositoryBase<Customer, Guid>
{
    protected override Guid CreateId()
        => Guid.NewGuid();
}
