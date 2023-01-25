using MinimalApis.Interfaces;

namespace MinimalApis.Repos;

public interface IRepository<TModel, in TId>
    where TModel : class, IIdentifiable<TId>
{
    IEnumerable<TModel> List();
    TModel? Get(TId id);
    TModel Create(TModel model);
    TModel? Update(TId id, TModel model);
    bool Delete(TId id);
}
