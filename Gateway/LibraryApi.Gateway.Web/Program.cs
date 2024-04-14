using System.Security.Cryptography.X509Certificates;
using LibraryApi.Gateway.Web;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
   .AddJsonFile("Config/ocelot.json", optional: false, reloadOnChange: true)
   .AddJsonFile($"Config/ocelot.{builder.Environment.EnvironmentName}.json", 
      optional: true, reloadOnChange: true);

builder.Services.ConfigureOcelot(builder.Configuration);
builder.Services.ConfigureAuthentication(builder.Configuration);

builder.Services.ConfigureDataProtection();

var app = builder.Build();

app.UseOcelot().Wait();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
