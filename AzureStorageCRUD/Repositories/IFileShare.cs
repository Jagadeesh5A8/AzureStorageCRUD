using AzureStorageCRUD.Models;

namespace AzureStorageCRUD.Repositories
{
    public interface IFileShare
    {
        Task FileUploadAsync(FileDetails fileDetails);
        Task<byte[]> FileDownloadAsync(string fileShareName);
        Task<bool> DeleteFileAsync(string fileShareName);
    }
}
