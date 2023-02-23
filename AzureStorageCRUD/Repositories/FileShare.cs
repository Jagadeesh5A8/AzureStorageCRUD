using Azure.Storage.Files.Shares.Models;
using Azure.Storage.Files.Shares;
using Azure;
using AzureStorageCRUD.Models;

namespace AzureStorageCRUD.Repositories
{
    public class FileShare:IFileShare
    {
        private readonly string _fileConnectionString;
        private readonly string _fileName;

        public FileShare(IConfiguration config)
        {
            _fileConnectionString = config.GetValue<string>("StorageConnectionString");
            _fileName = config.GetValue<string>("FileShareName");
        }

        public async Task<bool> FileUploadAsync(IFormFile file)
        {
            ShareClient share = new ShareClient(_fileConnectionString, _fileName);

            await share.CreateIfNotExistsAsync();

            if (await share.ExistsAsync())
            {
                ShareDirectoryClient directory = share.GetDirectoryClient("DemoFiles");

                await directory.CreateIfNotExistsAsync();

                if (await directory.ExistsAsync())
                {
                    ShareFileClient files = directory.GetFileClient(file.FileName);

                    using (var stream = file.OpenReadStream())
                    {
                        files.Create(stream.Length);
                        files.UploadRange(new HttpRange(0, stream.Length), stream);
                    }
                    return true;
                }
            }
            return false;
        }

        public async Task<byte[]> FileDownloadAsync(string fileShareName)
        {
            ShareClient share = new ShareClient(_fileConnectionString, _fileName);

            ShareDirectoryClient directory = share.GetDirectoryClient("DemoFiles");

            ShareFileClient file = directory.GetFileClient(fileShareName);

            ShareFileDownloadInfo download = file.Download();

            using (var stream = new MemoryStream())
            {
                await download.Content.CopyToAsync(stream);
                return stream.ToArray();
            }
        }

        public async Task<bool> DeleteFileAsync(string fileShareName)
        {
            ShareClient share = new ShareClient(_fileConnectionString, _fileName);

            ShareDirectoryClient directory = share.GetDirectoryClient("DemoFiles");

            ShareFileClient file = directory.GetFileClient(fileShareName);

            var result = await file.DeleteAsync();
            if(result.IsError == false)
            {
                return true;
            }
            return false;
        }
    }
}
