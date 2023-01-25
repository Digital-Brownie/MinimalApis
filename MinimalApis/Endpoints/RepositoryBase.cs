using MinimalApis.Interfaces;
using MinimalApis.Repos;

namespace MinimalApis.Endpoints;

public abstract class RepositoryBase<TModel, TId> : IRepository<TModel, TId>
where TModel : class, IIdentifiable<TId> where TId : notnull
{
    protected RepositoryBase()
    {
        DataSet = new();
    }

    protected readonly Dictionary<TId, TModel> DataSet;

    public IEnumerable<TModel> List()
        => DataSet.Values;

    public TModel? Get(TId id)
    {
        return DataSet.TryGetValue(id, out var value)
            ? value
            : null;
    }

    public TModel Create(TModel model)
    {
        var id = CreateId();
        model.Id = id;
        DataSet[id] = model;
        return model;
    }

    public TModel? Update(TId id, TModel model)
    {
        if (!DataSet.ContainsKey(id))
        {
            return null;
        }

        model.Id = id;
        DataSet[id] = model;
        return model;
    }

    public bool Delete(TId id)
    {
        if (!DataSet.ContainsKey(id))
        {
            return false;
        }

        DataSet.Remove(id);
        return true;
    }

    protected abstract TId CreateId();
}
