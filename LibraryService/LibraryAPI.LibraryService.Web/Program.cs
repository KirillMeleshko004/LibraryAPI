using LibraryAPI.LibraryService.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

#region Configure services

builder.Services.ConfigureCors();
builder.Services.ConfigureData(builder.Configuration);
builder.Services.ConfigureLogging();
builder.Services.ConfigureServices();

builder.Services.ConfigureControllers();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureDataProtection();
builder.Services.ConfigureAuthentication(builder.Configuration);

#endregion

var app = builder.Build();

#region Configure app pipeline

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseCors("default");
app.UseAuthentication();
app.UseAuthorization();

//Add endpoints for controllers
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v0/swagger.json", "Library API v0");
});

#endregion

app.Run();
