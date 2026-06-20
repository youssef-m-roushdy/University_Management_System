using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using University_Management_System.Application.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace University_Management_System.Infrastructure.Presistence.Services;

public class CloudinarySettings
{
    public string CloudName { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string ApiSecret { get; set; } = string.Empty;
}

/// <summary>
/// Cloudinary image upload service
/// </summary>
public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;
    private const long MaxImageSizeBytes = 2 * 1024 * 1024; // 2MB
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp"};
    private readonly string[] _allowedCourseExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

    public CloudinaryService(IOptions<CloudinarySettings> settings)
    {
        var account = new Account(
            settings.Value.CloudName,
            settings.Value.ApiKey,
            settings.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(account);
    }

    public string ExtractPublicIdFromUrl(string cloudinaryUrl)
    {
        var uri = new Uri(cloudinaryUrl);
        var path = uri.AbsolutePath;

        var uploadIndex = path.IndexOf("/upload/", StringComparison.Ordinal);
        if (uploadIndex < 0)
            throw new InvalidOperationException($"Invalid Cloudinary URL format: {cloudinaryUrl}");

        var afterUpload = path[(uploadIndex + 8)..];

        if (afterUpload.StartsWith("v") && afterUpload.Contains("/"))
        {
            var versionEnd = afterUpload.IndexOf('/');
            afterUpload = afterUpload[(versionEnd + 1)..];
        }

        var dotIndex = afterUpload.LastIndexOf('.');
        return dotIndex >= 0 ? afterUpload[..dotIndex] : afterUpload;
    }

    public async Task<string> UploadUserProfilePictureAsync(IFormFile file, string userId, CancellationToken cancellationToken = default)
    {
        ValidateFile(file);

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            Folder = "akhbaracademy/profiles",
            PublicId = $"user_{userId}_{Guid.NewGuid()}",
            Transformation = new Transformation()
                .Width(500)
                .Height(500)
                .Crop("fill")
                .Gravity("face")
                .Quality("auto")
                .FetchFormat("auto"),
            Overwrite = false
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

        if (uploadResult.Error != null)
        {
            throw new InvalidOperationException($"Image upload failed: {uploadResult.Error.Message}");
        }

        return uploadResult.SecureUrl.ToString();
    }
 

    public async Task<bool> DeleteImageAsync(string publicId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(publicId))
            return false;

        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);

        return result.Result == "ok";
    }

    private void ValidateFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File is required");
        }

        if (file.Length > MaxImageSizeBytes)
        {
            throw new ArgumentException($"File size exceeds the maximum limit of 2MB. Current size: {file.Length / 1024.0 / 1024.0:F2}MB");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_allowedExtensions.Contains(extension))
        {
            throw new ArgumentException($"File type '{extension}' is not allowed. Allowed types: {string.Join(", ", _allowedExtensions)}");
        }
    }

}
