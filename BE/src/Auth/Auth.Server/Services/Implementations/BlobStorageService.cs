using Auth.Server.Services.Abstractions;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Auth.Server.Services.Implementations;

public class BlobStorageService : IBlobStorageService
{
    private readonly string _storageConnectionString;
    private readonly string _storageContainerName;

    public BlobStorageService(IConfiguration configuration)
    {
        _storageConnectionString = configuration["Azure:BlobConnectionString"];
        _storageContainerName = configuration["Azure:AvatarContainerName"];
    }

    public async Task<string?> UploadAsync(IFormFile blob, string? blobName)
    {
        ValidateContentType(blob);

        var container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
        blobName ??= Guid.NewGuid().ToString();

        var blobHttpHeaders = new BlobHttpHeaders
        {
            ContentType = blob.ContentType
        };

        try
        {
            BlobClient client = container.GetBlobClient(blobName);
            await using (Stream? data = blob.OpenReadStream())
            {
                await client.UploadAsync(data, new BlobUploadOptions { HttpHeaders = blobHttpHeaders });
            }

            return client.Uri.ToString();
        }
        catch (RequestFailedException ex)
           when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
        {
            throw new Exception($"File with name {blob.FileName} already exists in container.");
        }

        catch (RequestFailedException ex)
        {
            throw new Exception($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
        }
    }

    public async Task DeleteAsync(string blobName)
    {
        var client = new BlobContainerClient(_storageConnectionString, _storageContainerName);

        BlobClient file = client.GetBlobClient(blobName);

        try
        {
            await file.DeleteAsync();
        }

        catch (RequestFailedException ex)
            when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
        {
            throw new Exception($"File with name {blobName} not found.");
        }
    }

    private void ValidateContentType(IFormFile blob)
    {
        if (blob.Length == 0)
        {
            throw new Exception("Invalid file.");
        }

        var allowedContentTypes = new List<string> { "image/jpeg", "image/png" };
        if (!allowedContentTypes.Contains(blob.ContentType))
        {
            throw new Exception($"Invalid content type.");
        }
    }
}
