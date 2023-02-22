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
        public async Task<IActionResult> Get()
        {
            try
            {
                var files = await _storage.ListAsync();
                if (files.Count == 0)
                    return NotFound();

                return StatusCode(StatusCodes.Status200OK, files);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost(nameof(Upload))]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                BlobResponseDto? response = await _storage.UploadAsync(file);

                if (response.Error == true)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response.Status);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{filename}")]
        public async Task<IActionResult> Download(string filename)
        {
            try
            {
                BlobRequestDto? file = await _storage.DownloadAsync(filename);

                if (file == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"File {filename} could not be downloaded.");
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
        public async Task<IActionResult> Delete(string filename)
        {
            try
            {
                BlobResponseDto response = await _storage.DeleteAsync(filename);

                if (response.Error == true)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response.Status);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, response.Status);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
