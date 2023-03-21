using AzureStorageCRUD.Entities;
using AzureStorageCRUD.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureStorageCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableStorageController : ControllerBase
    {
        private readonly ITableService _storageService;
        public TableStorageController(ITableService storageService)
        {
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        [HttpGet("Get")]
        [ActionName(nameof(GetAsync))]
        public async Task<IActionResult> GetAsync(string category, string id, string tableName)
        {
            try
            {
                var response = await _storageService.GetEntityAsync(category, id, tableName);

                if (response == null)
                    return BadRequest();

                return Ok(response);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> PostAsync([FromBody] GroceryItemEntity entity, string tableName)
        {
            try
            {
                entity.PartitionKey = entity.Category;

                string Id = Guid.NewGuid().ToString();
                entity.Id = Id;
                entity.RowKey = Id;

                var createdEntity = await _storageService.AddEntityAsync(entity,  tableName);

                if (createdEntity == null)
                    return BadRequest();

                return CreatedAtAction(nameof(GetAsync), createdEntity);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> PutAsync([FromBody] GroceryItemEntity entity, string tableName)
        {
            try
            {
                entity.PartitionKey = entity.Category;
                entity.RowKey = entity.Id;

                var response = await _storageService.UpsertEntityAsync(entity,  tableName);

                if (response == null) 
                    return BadRequest();
                return NoContent();
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync([FromQuery] string category, string id, string tableName)
        {
            try
            {
                var response = await _storageService.DeleteEntityAsync(category, id,tableName);

                if(response == true)
                    return NoContent();
                else 
                    return NotFound();
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
