using AzureStorageCRUD.Models;

namespace AzureStorageCRUD.Repositories
{
    public interface IFileShare
    {
        Task<bool> FileUploadAsync(IFormFile file, string fileName);
        Task<byte[]> FileDownloadAsync(string fileName, string fileShareName);
        Task<bool> DeleteFileAsync(string fileShareName, string fileName);
    }
}
