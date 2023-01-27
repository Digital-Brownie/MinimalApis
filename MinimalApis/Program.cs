using Microsoft.AspNetCore.Mvc;
using MinimalApis.Endpoints;
using MinimalApis.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointDefinitions(typeof(IEndpointDefinition))
    .RegisterServices<Program>();

var app = builder.Build();

app.Use(async (context, func) =>
{
    var query = context.Request.Query;
    await func();
});
app.MapGet("/test", TestQuery);

app
    .UseEndpointDefinitions()
    .Run();

object TestQuery([FromQuery] string query) => query;
