namespace Auth.Server.Services.Abstractions;

public interface IBlobStorageService
{
    Task<string?> UploadAsync(IFormFile file, string? blobName);
    Task DeleteAsync(string blobName);
}
