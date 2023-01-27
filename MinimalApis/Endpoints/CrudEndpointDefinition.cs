using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using MinimalApis.Interfaces;
using MinimalApis.Queries;
using MinimalApis.Repos;

namespace MinimalApis.Endpoints;

public abstract class CrudEndpointDefinition<TModel, TId, TQuery>
    where TModel : class, IIdentifiable<TId>
    where TQuery : QueryBase
{
    protected abstract string BaseRoute { get; }

    protected virtual void MapCrudEndpoints(WebApplication app)
    {
        MapDefaultQuery(app, "/").Produces<IEnumerable<TModel>>();
        MapQuery(app, "/query").Produces<IEnumerable<TModel>>();
        MapGet(app, "/{id:guid}").Produces<TModel>();
        MapPost(app, "/").Produces<TModel>();
        MapPut(app, "/{id:guid}").Produces<TModel>();
        MapDelete(app, "/{id:guid}").Produces(204);
    }

    protected virtual RouteHandlerBuilder MapGet(WebApplication app, [StringSyntax(StringSyntaxAttribute.Uri)] string pattern)
    {
        return app.MapGet($"{BaseRoute}{pattern}", Get);
    }

    protected virtual RouteHandlerBuilder MapDefaultQuery(WebApplication app, [StringSyntax(StringSyntaxAttribute.Uri)] string pattern)
    {
        return app.MapGet($"{BaseRoute}{pattern}", List);
    }

    protected virtual RouteHandlerBuilder MapQuery(WebApplication app, [StringSyntax(StringSyntaxAttribute.Uri)] string pattern)
    {
        return app.MapGet(
            $"{BaseRoute}{pattern}",
            (IRepository<TModel, TId> repository, TQuery query) => Query(repository, query)
            );
    }

    protected virtual RouteHandlerBuilder MapPost(WebApplication app, [StringSyntax(StringSyntaxAttribute.Uri)] string pattern)
    {
        return app.MapPost($"{BaseRoute}{pattern}", Create);
    }

    protected virtual RouteHandlerBuilder MapPut(WebApplication app, [StringSyntax(StringSyntaxAttribute.Uri)] string pattern)
    {
        return app.MapPut($"{BaseRoute}{pattern}", Update);
    }

    protected virtual RouteHandlerBuilder MapDelete(WebApplication app, [StringSyntax(StringSyntaxAttribute.Uri)] string pattern)
    {
        return app.MapDelete($"{BaseRoute}{pattern}", Delete);
    }

    protected IResult Created(TModel model)
        => Results.Created($"/{BaseRoute}/{model.Id}", model);

    protected IResult IsNotFound(TModel? model)
        => model switch
        {
            { } => Results.Ok(model),
            null => NotFound
        };

    protected IResult NotFound
        => Results.NotFound(new
        {
            Error = "4 OH 4!",
            Status = 404
        });

    protected virtual IEnumerable<TModel> Query([FromServices] IRepository<TModel, TId> repository, TQuery query)
    {
        var (count, page) = query;

        return repository.List(count, page);
    }

    protected virtual IEnumerable<TModel> List([FromServices] IRepository<TModel, TId> repository)
        => repository.List(10, 1);

    protected virtual IResult Get([FromServices] IRepository<TModel, TId> repository, TId id)
        => IsNotFound(repository.Get(id));

    protected virtual IResult Create([FromServices] IRepository<TModel, TId> repository, TModel customer)
        => Created(repository.Create(customer));

    protected virtual IResult Update([FromServices] IRepository<TModel, TId> repository, TId id, TModel customer)
        => IsNotFound(repository.Update(id, customer));

    protected virtual IResult Delete([FromServices] IRepository<TModel, TId> repository, TId id)
        => repository.Delete(id) ? Results.NoContent() : NotFound;
}
