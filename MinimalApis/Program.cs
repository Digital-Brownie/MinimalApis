using Microsoft.AspNetCore.Mvc;
using MinimalApis.Endpoints;
using MinimalApis.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointDefinitions(typeof(IEndpointDefinition));

var app = builder.Build();

app
    .UseEndpointDefinitions()
    .Run();
