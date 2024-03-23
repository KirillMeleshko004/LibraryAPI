using LibraryAPI.LibraryService.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCors();

builder.Services.ConfigureData(builder.Configuration);
builder.Services.ConfigureLogging();
builder.Services.ConfigureServices();

var app = builder.Build();

//Adding middlewares to pipeline

if(app.Environment.IsDevelopment())
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

app.Run();
