using CertCreator;

//CertCreator is a service, which creates development X509 certificates
//for authentication and data protection.
//Only for development purposes.

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<CertCreatorService>();


var host = builder.Build();
await host.RunAsync();