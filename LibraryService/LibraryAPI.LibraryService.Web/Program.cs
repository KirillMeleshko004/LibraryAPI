using LibraryAPI.LibraryService.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureLogging();
builder.Services.ConfigureCors();

//pass assembly with mapping profiles
builder.Services.AddAutoMapper(typeof(Program).Assembly);


var app = builder.Build();


app.Run();
