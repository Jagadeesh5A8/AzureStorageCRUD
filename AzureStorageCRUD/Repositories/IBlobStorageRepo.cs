using AzureStorageCRUD.Models;

namespace AzureStorageCRUD.Repositories
{
    public interface IBlobStorageRepo
    {
        Task<BlobResponseDto> UploadAsync(IFormFile file, string _storageContainerName);
        Task<BlobRequestDto> DownloadAsync(string blobFilename, string _storageContainerName);
        Task<bool> DeleteAsync(string blobFilename, string _storageContainerName);
        Task<List<BlobRequestDto>> ListAsync(string _storageContainerName);
    }
}
