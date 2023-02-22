using AzureStorageCRUD.Models;

namespace AzureStorageCRUD.Services
{
    public interface IQueueService
    {
        Task<string> SendMessage(string queueName, string message);
        Task<string?> UpdateMessage(string queueName);
        Task<string?> ReceiveMessage(string queueName);
        Task<int> ClearMessage(string queueName);
    }
}
