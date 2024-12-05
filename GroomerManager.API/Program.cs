using GroomerManager.Infrastructure.Persistence.Configurations;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.local.json");
}

builder.Services.AddOpenApi();

builder.Services.AddDatabaseConfiguration(builder.Configuration.GetConnectionString("GroomerManagerDB")!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World");

app.Run();
