using AzureStorageCRUD.Models;
using AzureStorageCRUD.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureStorageCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileShareController : ControllerBase
    {
        private readonly IFileShare fileShare;
        public FileShareController(IFileShare fileShare)
        {
            this.fileShare = fileShare;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                var result = await fileShare.FileUploadAsync(file);
                if(result == true)
                    return Ok();
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Download")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            try
            {
                var result = await fileShare.FileDownloadAsync(fileName);
                if (result != null)
                {
                    return File(result, "application/octet-stream", fileName);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            try
            {
                var result = await fileShare.DeleteFileAsync(fileName);
                if(result == true)
                    return NoContent();
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
