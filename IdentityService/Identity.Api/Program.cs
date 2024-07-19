using Identity.Api.Extensions;
using Identity.Controllers;
using Identity.Infrastructure;
using Identity.UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCors();
builder.Services.ConfigureLogger();
builder.Services.ConfigureOptions(builder.Configuration);

builder.Services.ConfigurePresentation();
builder.Services.ConfigureDataProtection();

builder.Services.ConfigureIdentity();
builder.Services.ConfigureOpenIdDict(builder.Configuration);

builder.Services.ConfigureUseCases();
builder.Services.ConfigureInfrastructureServices(builder.Configuration);

var app = builder.Build();

#region Configure app pipeline

var logger = app.Services.GetRequiredService<ILogger<Program>>();
app.ConfigureExceptionHandler(logger);

if (app.Environment.IsProduction())
{
   app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors("Default");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
