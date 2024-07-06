using Identity.Api.Extensions;
using Identity.UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureLogger();
builder.Services.ConfigureCors();
builder.Services.ConfigureData(builder.Configuration);

builder.Services.ConfigurePresentationControllers();
builder.Services.ConfigureSwagger();

builder.Services.ConfigureUseCases();

builder.Services.ConfigureOpenIdDict(builder.Configuration);

builder.Services.ConfigureIdentity();

builder.Services.ConfigureOptions(builder.Configuration);
builder.Services.ConfigureDataProtection();

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

app.UseSwagger();
app.UseSwaggerUI(options =>
{
   options.SwaggerEndpoint("/swagger/v0/swagger.json", "Identity API v0");
});

#endregion

app.Run();
