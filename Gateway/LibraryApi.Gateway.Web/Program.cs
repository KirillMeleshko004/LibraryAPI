using LibraryApi.Gateway.Web;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.ConfigureOcelot(builder.Configuration);
builder.Services.ConfigureAuthentication(builder.Configuration);

var app = builder.Build();

app.UseOcelot().Wait();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
