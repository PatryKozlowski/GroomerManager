using System.Text;
using GroomerManager.API.Exception;
using GroomerManager.Application;
using GroomerManager.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var tokenSecretKey = builder.Configuration.GetValue<string>("Jwt:Secret");

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.local.Development.json");
}

builder.AddServiceDefaults();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        if (tokenSecretKey != null)
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "groomermanager.pl",
                ValidAudience = "groomermanager.pl",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecretKey))
            };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseExceptionHandler(_ => { });

app.UseAuthentication();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "GroomerManager Demo API");
    });
}

app.UseHttpsRedirection();

app.MapDefaultEndpoints();
app.MapControllers();

// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//     dbContext.Database.Migrate();
// }

app.Run();
