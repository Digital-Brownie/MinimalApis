using Microsoft.AspNetCore.Mvc;
using MinimalApis.Endpoints;
using MinimalApis.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointDefinitions(typeof(IEndpointDefinition))
    .RegisterServices<Program>();

var app = builder.Build();

builder
    .Build()
    .UseEndpointDefinitions()
    .Run();
