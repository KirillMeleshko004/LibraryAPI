
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
        Directory.CreateDirectory(_configuration.GetValue<string>("CERT_DIR")!);

        CreateHttpsCertificate();
        CreateEncryptionCertificate();
        CreateSigningCertificate();

        await Task.CompletedTask;

        _appLifetime.StopApplication();
    }

    private static bool IsCertExistAndValid(string path, X509KeyUsageFlags flag)
    {
        if (!File.Exists(path!))
        {
            return false;
        }

        var bytes = File.ReadAllBytes(path);
        var cert = new X509Certificate2(bytes);

        if (cert.NotAfter <= DateTime.Now)
        {
            return false;
        }

        List<X509KeyUsageExtension> extensions = cert.Extensions
            .OfType<X509KeyUsageExtension>().ToList();

        if (!extensions.Any(f => f.KeyUsages.Equals(flag)))
        {
            return false;
        }

        return true;
    }

    private void CreateEncryptionCertificate()
    {
        var path = _configuration.GetValue<string>("ENCRYPTION_CERT_PATH")!;

        if (IsCertExistAndValid(path, X509KeyUsageFlags.KeyEncipherment))
        {
            return;
        }

        using var algorithm = RSA.Create(keySizeInBits: 2048);
        var subject = new X500DistinguishedName("CN=Development Encryption Certificate");
        var request = new CertificateRequest(subject, algorithm,
            HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        request.CertificateExtensions
            .Add(new X509KeyUsageExtension(X509KeyUsageFlags.KeyEncipherment, critical: true));
        var encCertificate =
            request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));
        File.WriteAllBytes(path!,
            encCertificate.Export(X509ContentType.Pfx, string.Empty));
    }

    private void CreateSigningCertificate()
    {
        var path = _configuration.GetValue<string>("SINGING_CERT_PATH")!;

        if (IsCertExistAndValid(path, X509KeyUsageFlags.DigitalSignature))
        {
            return;
        }

        using var algorithm = RSA.Create(keySizeInBits: 2048);
        var subject = new X500DistinguishedName("CN=Development Signing Certificate");
        var request = new CertificateRequest(subject, algorithm,
            HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        request.CertificateExtensions
            .Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, critical: true));
        var encCertificate =
            request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));

        File.WriteAllBytes(path!,
            encCertificate.Export(X509ContentType.Pfx, string.Empty));
    }

    private void CreateHttpsCertificate()
    {
        var path = _configuration.GetValue<string>("DP_CERT_PATH")!;

        if (IsCertExistAndValid(path, X509KeyUsageFlags.DataEncipherment
                | X509KeyUsageFlags.KeyEncipherment
                | X509KeyUsageFlags.DigitalSignature))
        {
            return;
        }

        using var algorithm = RSA.Create(keySizeInBits: 2048);
        var subject = new X500DistinguishedName("CN=Development Data Protection Certificate");
        var request = new CertificateRequest(subject, algorithm,
            HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        request.CertificateExtensions
            .Add(new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment
                | X509KeyUsageFlags.KeyEncipherment
                | X509KeyUsageFlags.DigitalSignature,
                critical: true));
        var encCertificate =
            request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));

        File.WriteAllBytes(path!,
            encCertificate.Export(X509ContentType.Pfx, string.Empty));
    }
}
