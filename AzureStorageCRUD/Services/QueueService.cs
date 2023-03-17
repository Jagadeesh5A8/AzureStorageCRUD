using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace AzureStorageCRUD.Services
{
    public class QueueService:IQueueService
    {
        private readonly string _storageConnectionString;
        public QueueService(IConfiguration configuration)
        {
            _storageConnectionString = configuration.GetValue<string>("StorageConnectionString");
        }
        public async Task<string> SendMessage(string queueName, string message)
        {
            var options = new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            };
            var queueClient = new QueueClient(_storageConnectionString, queueName, options);

            await queueClient.CreateIfNotExistsAsync();

            if (queueClient.Exists())
            {
                await queueClient.SendMessageAsync(message);
            }

            return message;
        }

        public async Task<string?> UpdateMessage(string queueName, string messageName)
        {
            var queueClient = new QueueClient(_storageConnectionString, queueName);

            if (queueClient.Exists())
            {
                QueueMessage[] message = await queueClient.ReceiveMessagesAsync();

                await queueClient.UpdateMessageAsync(message[0].MessageId,
                        message[0].PopReceipt,
                        messageName,
                        TimeSpan.FromSeconds(2.0)
                    );
                return message[0].Body.ToString();
            }
            return null;
        }

        public async Task<string?> ReceiveMessage(string queueName)
        {
            QueueMessage[] retrievedMessage;
            
            QueueClient queueClient = new QueueClient(_storageConnectionString, queueName);

            if (queueClient.Exists())
            {
                QueueProperties properties = await queueClient.GetPropertiesAsync();

                if (properties.ApproximateMessagesCount > 0)
                {
                    retrievedMessage = await queueClient.ReceiveMessagesAsync(1);
                    string message = retrievedMessage[0].MessageText;
                    await queueClient.DeleteMessageAsync(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
                    return message;
                }

                return null;
            }
            return null;
        }

        public async Task<int> ClearMessage(string queueName)
        {
            var queueClient = new QueueClient(_storageConnectionString, queueName);
            if (await queueClient.ExistsAsync())
            {
                var response = await queueClient.ClearMessagesAsync();
                return response.Status;
            }
            return 0;
        }
    }
}
