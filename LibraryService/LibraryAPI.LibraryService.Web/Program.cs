using LibraryAPI.LibraryService.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureLogging();
builder.Services.ConfigureCors();


var app = builder.Build();


app.Run();
