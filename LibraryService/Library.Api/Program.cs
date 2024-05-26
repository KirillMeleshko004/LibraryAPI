var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMediatR(config => 
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly));


var app = builder.Build();


app.Run();
