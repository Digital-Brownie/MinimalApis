namespace MinimalApis.Utils;

[AttributeUsage(AttributeTargets.Class)]
public class InjectedAttribute : Attribute
{
    public ServiceLifetime Lifetime { get; }

    public InjectedAttribute(ServiceLifetime lifetime)
    {
        Lifetime = lifetime;
    }
}

public class InjectedAttribute<TInterface> : InjectedAttribute
{
    public InjectedAttribute(ServiceLifetime lifetime) : base(lifetime)
    {
        RegisteredType = typeof(TInterface);
    }

    public Type RegisteredType { get; }
}
