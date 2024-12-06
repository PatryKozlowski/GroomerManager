using GroomerManager.API.Auth;
using GroomerManager.API.Endpoints;
using GroomerManager.API.Exception;
using GroomerManager.Application;
using GroomerManager.Application.Interfaces;
using GroomerManager.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.local.json");
}

builder.Services.AddOpenApi();

builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<CookieSettings>(builder.Configuration.GetSection("CookieSettings"));
builder.Services.AddScoped<IAuthenticationDataProvider, JwtDataProvider>();

var app = builder.Build();

app.UseExceptionHandler(_ => {});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.AddEndpoints();

app.Run();
