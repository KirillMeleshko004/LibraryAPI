using CertCreator;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<CertCreatorService>();

var host = builder.Build();
await host.RunAsync();