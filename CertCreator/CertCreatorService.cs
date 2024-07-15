
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CertCreator;

public class CertCreatorService : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IHostApplicationLifetime _appLifetime;

    public CertCreatorService(IConfiguration configuration,
        IHostApplicationLifetime appLifetime)
    {
        _configuration = configuration;
        _appLifetime = appLifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //TODO
        //Copy certs to shared folder
        Directory.CreateDirectory("Certs");
        CreateEncryptionCertificate();
        CreateSigningCertificate();

        await Task.CompletedTask;

        _appLifetime.StopApplication();
    }

    private void CreateEncryptionCertificate()
    {
        using var algorithm = RSA.Create(keySizeInBits: 2048);
        var subject = new X500DistinguishedName("CN=Development Encryption Certificate");
        var request = new CertificateRequest(subject, algorithm,
            HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        request.CertificateExtensions
            .Add(new X509KeyUsageExtension(X509KeyUsageFlags.KeyEncipherment, critical: true));
        var encCertificate =
            request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));
        var path = _configuration.GetSection("Certs").GetValue<string>("ENCRYPTION_CERT_PATH");
        File.WriteAllBytes(path!,
            encCertificate.Export(X509ContentType.Pfx, string.Empty));
    }

    private void CreateSigningCertificate()
    {
        using var algorithm = RSA.Create(keySizeInBits: 2048);
        var subject = new X500DistinguishedName("CN=Development Signing Certificate");
        var request = new CertificateRequest(subject, algorithm,
            HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        request.CertificateExtensions
            .Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, critical: true));
        var encCertificate =
            request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));
        var path = _configuration.GetSection("Certs").GetValue<string>("SINGING_CERT_PATH");
        File.WriteAllBytes(path!,
            encCertificate.Export(X509ContentType.Pfx, string.Empty));
    }
}
