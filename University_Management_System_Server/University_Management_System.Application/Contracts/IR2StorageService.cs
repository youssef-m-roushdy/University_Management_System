using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace University_Management_System.Application.Contracts
{
    public interface IR2StorageService
    {
        Task<string> UploadAsync(IFormFile file, string key, CancellationToken ct = default);
        Task<string> GetSignedUrlAsync(string key, TimeSpan expiry, CancellationToken ct = default);
        Task<bool> DeleteAsync(string key, CancellationToken ct = default);
        string ExtractKeyFromUrl(string url);
    }
}