using LibraryAPI.LibraryService.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCors();

builder.Services.ConfigureData(builder.Configuration);
builder.Services.ConfigureLogging();
builder.Services.ConfigureServices();

builder.Services.ConfigureControllers();
builder.Services.ConfigureSwagger();

var app = builder.Build();

//Adding middlewares to pipeline

if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v0/swagger.json", "Library API v0");
    });
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

app.Run();
