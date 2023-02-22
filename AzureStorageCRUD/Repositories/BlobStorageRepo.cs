using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureStorageCRUD.Models;

namespace AzureStorageCRUD.Repositories
{ 
    public class BlobStorageRepo : IBlobStorageRepo
    {
        private readonly string _storageConnectionString;
        private readonly string _storageContainerName;

        public BlobStorageRepo(IConfiguration configuration)
        {
            _storageConnectionString = configuration.GetValue<string>("StorageConnectionString");
            _storageContainerName = configuration.GetValue<string>("BlobContainerName");
        }
        public async Task<List<BlobRequestDto>> ListAsync()
        {
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);

            var files = new List<BlobRequestDto>();

            await foreach (BlobItem file in container.GetBlobsAsync())
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

        public async Task<BlobResponseDto> UploadAsync(IFormFile blob)
        {
            BlobResponseDto response = new();

            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
            //await container.CreateAsync();
            try
            {
                BlobClient client = container.GetBlobClient(blob.FileName);

                await using (Stream? data = blob.OpenReadStream())
                {
                    await client.UploadAsync(data);
                }

                response.Status = $"File {blob.FileName} Uploaded Successfully";
                response.Error = false;
                response.Blob.Uri = client.Uri.AbsoluteUri;
                response.Blob.Name = client.Name;

            }
            catch (RequestFailedException ex)
               when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                response.Status = $"File with name {blob.FileName} already exists. Please use another name to store your file.";
                response.Error = true;
                return response;
            }
            catch (RequestFailedException ex)
            {
                response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
                response.Error = true;
                return response;
            }
            return response;
        }
        public async Task<BlobRequestDto> DownloadAsync(string blobFilename)
        {
            BlobContainerClient client = new BlobContainerClient(_storageConnectionString, _storageContainerName);

            try
            {
                BlobClient file = client.GetBlobClient(blobFilename);

                if (await file.ExistsAsync())
                {
                    var data = await file.OpenReadAsync();
                    Stream blobContent = data;

                    var content = await file.DownloadContentAsync();

                    string name = blobFilename;
                    string contentType = content.Value.Details.ContentType;

                    return new BlobRequestDto { Content = blobContent, Name = name, ContentType = contentType };
                }
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                return (BlobRequestDto)ex.Data;
            }
            return null;
        }
        public async Task<BlobResponseDto> DeleteAsync(string blobFilename)
        {
            BlobContainerClient client = new BlobContainerClient(_storageConnectionString, _storageContainerName);

            BlobClient file = client.GetBlobClient(blobFilename);

            try
            {
                await file.DeleteAsync();
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                return new BlobResponseDto { Error = true, Status = $"File with name {blobFilename} not found." };
            }
            return new BlobResponseDto { Error = false, Status = $"File: {blobFilename} has been successfully deleted." };

        }
    }
}
