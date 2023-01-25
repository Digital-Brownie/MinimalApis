var builder = WebApplication.CreateBuilder();

builder.Logging.AddConsole().SetMinimumLevel(LogLevel.Trace);

builder.Services.AddLogging();
builder.Services.AddTransient<ILogger>(s => s.GetService<ILogger<Program>>()!);

var app = builder.Build();

app.Use(async (context, func) =>
{
    var logger = context.RequestServices.GetService<ILogger>();
    logger!.LogInformation(">>> Incoming API request {Method} {Path}", context.Request.Method, context.Request.Path);

    if (context.Request.Method == "POST")
    {
        var request = await context.Request.ReadFromJsonAsync<object>();
        logger!.LogDebug("Request {Request}", request);
    }

    await func();
});

app.Use(async (context, func) =>
{
    // if (!context.Request.IsHttps)
    // {
    //     context.Response.StatusCode = 400;
    //     await context.Response.WriteAsync("Oh fok!");
    //     return;
    // }
    await func();
});

app.MapGet("hello-world", () => "Hello, World!");
app.MapPost("create", (object request) => $"request: {request}");

app.Run();
