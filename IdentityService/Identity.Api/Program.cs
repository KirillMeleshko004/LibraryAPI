using Identity.Api.Extensions;
using Identity.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureLogger();
builder.Services.ConfigureCors();
builder.Services.ConfigureData(builder.Configuration);

builder.Services.ConfigurePresentationControllers();
builder.Services.ConfigureSwagger();

builder.Services.ConfigureApplicationServices();

//Configure Identity !!!BEFORE!!! Authentication
//Another order may cause Identity default authentication schema override JWT schema
builder.Services.ConfigureIdentity();
builder.Services.ConfigureAuthentication(builder.Configuration);

builder.Services.ConfigureOptions(builder.Configuration);
builder.Services.ConfigureDataProtection();

var app = builder.Build();

#region Configure app pipeline

if (app.Environment.IsDevelopment())
{
   app.UseDeveloperExceptionPage();
}
else
{
   app.UseMiddleware<ExceptionMiddleware>();
   app.UseHsts();
}

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
