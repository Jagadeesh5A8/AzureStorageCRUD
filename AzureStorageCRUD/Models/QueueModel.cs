namespace AzureStorageCRUD.Models
{
    public class QueueModel
    {
        public string QueueName;
        public string MessageId;
        public BinaryData MessageText;
        public string PopReceipt;
    }
}
