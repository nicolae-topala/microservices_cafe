namespace Auth.Server;

public class IdentityCertificates
{
    public string Path { get; set; } = string.Empty;
    public string CertExtension { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public CertificateInfo SigningCertificate { get; set; }
}

public class CertificateInfo
{
    public string Thumbprint { get; set; } = string.Empty;
}