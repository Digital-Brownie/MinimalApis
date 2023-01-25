using System.Reflection;
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

        services.AddSingleton(endpointDefinitions as IReadOnlyCollection<IEndpointDefinition>);

        return services;
    }

    public static IServiceCollection RegisterServices<TMarker>(this IServiceCollection services, params Assembly[] assemblies)
    {
        var lassemblies = assemblies.ToHashSet();
        lassemblies.Add(typeof(TMarker).Assembly);

        var allTypes = lassemblies.SelectMany(
                assembly => assembly
                    .DefinedTypes
                    .Where(t => Attribute.IsDefined(t.AsType(), typeof(InjectedAttribute)))
                    .Select(t => (Type: t.AsType(), Atrribute: t.GetCustomAttribute<InjectedAttribute>()))
            )
            .ToList();

        var genericTypes = lassemblies.SelectMany(
                assembly => assembly
                    .DefinedTypes
                    .Where(t => Attribute.IsDefined(t.AsType(), typeof(InjectedAttribute<>)))
                    .Select(t => (Type: t.AsType(), Atrribute: t.GetCustomAttribute<InjectedAttribute>()))
            )
            .ToList();

        var concreteTypes = from type in allTypes
            join interfaceType in genericTypes on type.Type equals interfaceType.Type into temp
            from registeredInterfaceType in temp.DefaultIfEmpty()
            where registeredInterfaceType == default
            select new ServiceDescriptor(type.Type, type.Type, type.Atrribute.Lifetime);

        var interfaceTypes = from type in allTypes
            join interfaceType in genericTypes on type.Type equals interfaceType.Type into temp
            from registeredInterfaceType in temp.DefaultIfEmpty()
            where registeredInterfaceType != default
            select new ServiceDescriptor(
                registeredInterfaceType.Atrribute.GetRegisteredTypeFromAttribute(),
                registeredInterfaceType.Type,
                type.Atrribute.Lifetime
            );

        concreteTypes
            .Concat(interfaceTypes)
            .ToList()
            .ForEach(services.Add);

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

    private static Type GetRegisteredTypeFromAttribute(this InjectedAttribute attribute)
    {
        return (attribute.GetType().GetProperty(nameof(InjectedAttribute<object>.RegisteredType))!
            .GetValue(attribute) as Type)!;
    }
}
