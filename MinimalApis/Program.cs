using Microsoft.AspNetCore.Mvc;
using MinimalApis.Endpoints;
using MinimalApis.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointDefinitions(typeof(IEndpointDefinition));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app
    .UseEndpointDefinitions()
    .Run();
