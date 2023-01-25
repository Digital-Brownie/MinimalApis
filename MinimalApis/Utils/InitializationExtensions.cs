using MinimalApis.Endpoints;

namespace MinimalApis.Utils;

public static class InitializationExtensions
{
    public static IServiceCollection AddEndpointDefinitions(this IServiceCollection services, Type type)
    {
        var endpointDefinitions = type
            .Assembly
            .DefinedTypes
            .Where(t => typeof(IEndpointDefinition).IsAssignableFrom(t))
            .Where(t => !t.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IEndpointDefinition>()
            .ToList();

        endpointDefinitions.ForEach(e => e.DefineServices(services));

        services.AddSingleton(endpointDefinitions as IReadOnlyCollection<IEndpointDefinition>);

        return services;
    }

    public static WebApplication UseEndpointDefinitions(this WebApplication app)
    {
        app
            .Services
            .GetRequiredService<IReadOnlyCollection<IEndpointDefinition>>()
            .ToList()
            .ForEach(e => e.DefineEndpoints(app));

        return app;
    }
}
