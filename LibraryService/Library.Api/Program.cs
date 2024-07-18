using Library.Api.Extensions;
using Library.Infrastructure;
using Library.Infrastructure.Images;
using Library.Api.Controllers;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

#region Configure services

builder.Services.ConfigureCors();
builder.Services.ConfigureEF(builder.Configuration);
builder.Services.ConfigureLogging();

builder.Services.ConfigureControllers();
builder.Services.ConfigurePresentation();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureDataProtection();

builder.Services.ConfigureOpenIdDict(builder.Configuration);
builder.Services.ConfigurePolicies();

builder.Services.ConfigureApplicationServices(builder.Configuration);
builder.Services.ConfigureInfrastructureServices();

#endregion


var app = builder.Build();

#region Configure app pipeline

var logger = app.Services.GetRequiredService<ILogger<Program>>();
app.ConfigureExceptionHandler(logger);

if (!app.Environment.IsDevelopment())
{
   app.UseHsts();
}

app.UseCors("default");
app.UseAuthentication();
app.UseAuthorization();

//Add endpoints for controllers
app.MapControllers();


var imageOptions = new ImageOptions();
builder.Configuration.Bind(ImageOptions.SectionName, imageOptions);
app.UseStaticFiles(new StaticFileOptions()
{
   RequestPath = "/images",
   FileProvider = new PhysicalFileProvider(Path.Combine(
      Directory.GetCurrentDirectory(),
      imageOptions.StorePath)),
   OnPrepareResponse = ctx =>
   {
      ctx.Context.Response.Headers.Append(
         "Cache-Control", $"public, max-age={builder.Configuration
            .GetValue<string>("Cache:MaxAge")}");
   }
});

app.UseSwagger();
app.UseSwaggerUI(options =>
{
   options.SwaggerEndpoint("/swagger/v0/swagger.json", "Library API v0");
});

#endregion

app.Run();
