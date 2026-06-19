using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using University_Management_System.Application.Contracts;

namespace University_Management_System.Infrastructure.Presistence.Services
{
    public class R2StorageService : IR2StorageService
    {
        private readonly IAmazonS3 _s3Client; // R2 is S3-compatible
        private readonly string _bucketName;

        public async Task<string> UploadAsync(IFormFile file, string key, CancellationToken ct = default)
        {
            using var stream = file.OpenReadStream();
            await _s3Client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = stream,
                ContentType = file.ContentType
            }, ct);

            return key; // return the key, not a URL
        }

        public Task<string> GetSignedUrlAsync(string key, TimeSpan expiry, CancellationToken ct = default)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = key,
                Expires = DateTime.UtcNow.Add(expiry),
                Verb = HttpVerb.GET
            };

            return Task.FromResult(_s3Client.GetPreSignedURL(request));
        }

        public async Task<bool> DeleteAsync(string key, CancellationToken ct = default)
        {
            var response = await _s3Client.DeleteObjectAsync(_bucketName, key, ct);
            return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}