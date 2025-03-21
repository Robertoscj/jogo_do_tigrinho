using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace TigrinhoGame.Infrastructure.Services.AWS
{
    public interface IAwsStorageService
    {
        Task<string> UploadFileAsync(string fileName, Stream fileStream);
        Task<Stream> DownloadFileAsync(string fileName);
        Task DeleteFileAsync(string fileName);
    }

    public class AwsStorageService : IAwsStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly string _cloudFrontUrl;

        public AwsStorageService(IAmazonS3 s3Client, IConfiguration configuration)
        {
            _s3Client = s3Client;
            _bucketName = configuration["AWS:S3:BucketName"];
            _cloudFrontUrl = configuration["AWS:S3:CloudFrontUrl"];
        }

        public async Task<string> UploadFileAsync(string fileName, Stream fileStream)
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName,
                InputStream = fileStream,
                ContentType = GetContentType(fileName)
            };

            await _s3Client.PutObjectAsync(putRequest);

            // Retorna a URL do CloudFront para o arquivo
            return $"{_cloudFrontUrl}/{fileName}";
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            var getRequest = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };

            var response = await _s3Client.GetObjectAsync(getRequest);
            return response.ResponseStream;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };

            await _s3Client.DeleteObjectAsync(deleteRequest);
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".pdf" => "application/pdf",
                ".json" => "application/json",
                _ => "application/octet-stream"
            };
        }
    }
} 