using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;

namespace SimpleQueue.Infrastructure
{
    public class AzureStorage : IAzureStorage
    {
        private readonly string _storageConnectionString;
        private readonly string _storageContainerName;
        private readonly ILogger<AzureStorage> _logger;

        public AzureStorage(IConfiguration configuration, ILogger<AzureStorage> logger)
        {
            _storageConnectionString = configuration.GetValue<string>("AzureStorage:blobStorage");
            _storageContainerName = configuration.GetValue<string>("AzureStorage:blobContainerName");
            _logger = logger;
        }

        public Task<ImageBlob> DeleteAsync(string blobFilename)
        {
            throw new NotImplementedException();
        }

        public Task<ImageBlob> DownloadAsync(string blobFilename)
        {
            throw new NotImplementedException();
        }

        public Task<List<ImageBlob>> ListAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ImageBlob> UploadAsync(IFormFile blob)
        {
            ImageBlob imageBlob = new();

            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
            
            try
            {
                BlobClient client = container.GetBlobClient(blob.FileName);

                await using (Stream? data = blob.OpenReadStream())
                {
                    await client.UploadAsync(data);
                }

                imageBlob.Name = blob.Name;
                imageBlob.ImageLink = client.Uri.AbsoluteUri;
            }
            catch (RequestFailedException ex)
               when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                _logger.LogError($"File with name {blob.FileName} already exists in container. Set another name to store the file in the container: '{_storageContainerName}.'");
                
                return await Task.FromResult<ImageBlob>(null);
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
                
                return await Task.FromResult<ImageBlob>(null);
            }

            return imageBlob;
        }
    }
}
