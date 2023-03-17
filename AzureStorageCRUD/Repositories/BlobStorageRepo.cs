using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureStorageCRUD.Models;

namespace AzureStorageCRUD.Repositories
{ 
    public class BlobStorageRepo : IBlobStorageRepo
    {
        private readonly string _storageConnectionString;
        //private readonly string _storageContainerName;

        public BlobStorageRepo(IConfiguration configuration)
        {
            _storageConnectionString = configuration.GetValue<string>("StorageConnectionString");
            // _storageContainerName = configuration.GetValue<string>("BlobContainerName");
        }
        public async Task<List<BlobRequestDto>> ListAsync(string _storageContainerName)
        {
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);

            var files = new List<BlobRequestDto>();

            var blobs = container.GetBlobsAsync();

            await foreach (BlobItem file in blobs)
            {
                string uri = container.Uri.ToString();
                var name = file.Name;
                var fullUri = $"{uri}/{name}";

                files.Add(new BlobRequestDto
                {
                    Uri = fullUri,
                    Name = name,
                    ContentType = file.Properties.ContentType
                });
            }
            return files;
        }

        public async Task<BlobResponseDto> UploadAsync(IFormFile file, string _storageContainerName)
        {
            BlobResponseDto response = new();

            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);

            BlobClient client = container.GetBlobClient(file.FileName);

            await using (Stream? data = file.OpenReadStream())
            {
                await client.UploadAsync(data);
            }

            response.Status = $"File {file.FileName} Uploaded Successfully";
            response.Error = false;
            response.Blob.Uri = client.Uri.AbsoluteUri;
            response.Blob.Name = client.Name;

            return response;
        }
        public async Task<BlobRequestDto> DownloadAsync(string filename, string _storageContainerName)
        {
            BlobContainerClient client = new BlobContainerClient(_storageConnectionString, _storageContainerName);

            BlobClient file = client.GetBlobClient(filename);

            if (await file.ExistsAsync())
            {

                var data = await file.OpenReadAsync();

                var content = await file.DownloadContentAsync();

                string name = filename;
                string contentType = content.Value.Details.ContentType;

                return new BlobRequestDto { Content = data, Name = name, ContentType = contentType };
            }
            return null;
        }
        public async Task<bool> DeleteAsync(string filename, string _storageContainerName)
        {
            BlobContainerClient client = new BlobContainerClient(_storageConnectionString, _storageContainerName);

            BlobClient file = client.GetBlobClient(filename);

            var result = await file.DeleteAsync();
            if (result.IsError == true)
            {
                return false;
            }
            return true;
        }
    }
}
