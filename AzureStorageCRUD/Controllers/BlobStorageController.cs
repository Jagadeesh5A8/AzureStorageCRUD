using AzureStorageCRUD.Models;
using AzureStorageCRUD.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureStorageCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobStorageController : ControllerBase
    {
        private readonly IBlobStorageRepo _storage;

        public BlobStorageController(IBlobStorageRepo storage)
        {
            _storage = storage;
        }

        [HttpGet(nameof(Get))]
        public async Task<IActionResult> Get(string StorageContainerName)
        {
            try
            {
                var files = await _storage.ListAsync(StorageContainerName);

                if (files.Count == 0)
                    return NotFound();

                return Ok(files);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost(nameof(Upload))]
        public async Task<IActionResult> Upload(IFormFile file, string StorageContainerName)
        {
            try
            {
                BlobResponseDto? response = await _storage.UploadAsync(file, StorageContainerName);

                if (response.Error == true)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(Download))]
        public async Task<IActionResult> Download(string filename, string StorageContainerName)
        {
            try
            {
                BlobRequestDto? file = await _storage.DownloadAsync(filename, StorageContainerName);

                if (file == null)
                {
                    return NotFound();
                }
                else
                {
                    return File(file.Content, file.ContentType, file.Name);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string filename, string StorageContainerName)
        {
            try
            {
                var response = await _storage.DeleteAsync(filename, StorageContainerName);

                if (response == false)
                {
                    return NotFound();
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
