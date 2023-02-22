namespace AzureStorageCRUD.Models
{
    public class BlobResponseDto
    {
        public string? Status { get; set; }
        public bool Error { get; set; }
        public BlobRequestDto Blob { get; set; }

        public BlobResponseDto()
        {
            Blob = new BlobRequestDto();
        }
    }
}
