using AzureStorageCRUD.Models;

namespace AzureStorageCRUD.Repositories
{
    public interface IBlobStorageRepo
    {
        Task<BlobResponseDto> UploadAsync(IFormFile file);
        Task<BlobRequestDto> DownloadAsync(string blobFilename);
        Task<BlobResponseDto> DeleteAsync(string blobFilename);
        Task<List<BlobRequestDto>> ListAsync();
    }
}
