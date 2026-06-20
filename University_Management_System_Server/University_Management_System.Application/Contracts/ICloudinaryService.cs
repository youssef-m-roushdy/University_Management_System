using Microsoft.AspNetCore.Http;

namespace University_Management_System.Application.Contracts
{
    public interface ICloudinaryService
    {
        Task<string> UploadUserProfilePictureAsync(IFormFile file, string userId, CancellationToken cancellationToken = default);
        Task<bool> DeleteImageAsync(string publicId, CancellationToken cancellationToken = default);
        string ExtractPublicIdFromUrl(string cloudinaryUrl);
    }
}
