using AzureStorageCRUD.Models;

namespace AzureStorageCRUD.Repositories
{
    public interface IFileShare
    {
        Task<bool> FileUploadAsync(IFormFile file);
        Task<byte[]> FileDownloadAsync(string fileShareName);
        Task<bool> DeleteFileAsync(string fileShareName);
    }
}
