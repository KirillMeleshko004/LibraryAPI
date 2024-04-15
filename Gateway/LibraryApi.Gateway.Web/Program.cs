using LibraryApi.Gateway.Web;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
   // .AddJsonFile("Config/ocelot.json", optional: false, reloadOnChange: true)
   .AddJsonFile($"Config/ocelot.{builder.Environment.EnvironmentName}.json",
      optional: true, reloadOnChange: true);

builder.Services.ConfigureOcelot(builder.Configuration);
builder.Services.ConfigureAuthentication(builder.Configuration);

builder.Services.ConfigureDataProtection();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerForOcelot(builder.Configuration);

var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();

app.UseSwaggerForOcelotUI(options =>
{
   options.PathToSwaggerGenerator = "/swagger/docs";
});
app.UseOcelot().Wait();

app.Run();
