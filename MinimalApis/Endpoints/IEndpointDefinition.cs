﻿namespace MinimalApis.Endpoints;

public interface IEndpointDefinition
{
    void DefineServices(IServiceCollection services);
    void DefineEndpoints(WebApplication app);
}