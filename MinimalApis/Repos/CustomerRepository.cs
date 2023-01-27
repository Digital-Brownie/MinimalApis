using MinimalApis.Models;

namespace MinimalApis.Repos;


public class CustomerRepository : RepositoryBase<Customer, Guid>
{
    protected override Guid CreateId()
        => Guid.NewGuid();
}
