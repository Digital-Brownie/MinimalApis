using MinimalApis.Endpoints;
using MinimalApis.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointDefinitions(typeof(IEndpointDefinition))
    .RegisterServices<Program>();

builder
    .Build()
    .UseEndpointDefinitions()
    .Run();
