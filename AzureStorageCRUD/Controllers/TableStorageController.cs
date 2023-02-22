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
        [HttpGet]
        [ActionName(nameof(GetAsync))]
        public async Task<IActionResult> GetAsync(string tableName,string category, string id)
        {
            try
            {
                var response = await _storageService.GetEntityAsync(tableName,category, id);
                if (response == null)
                    return BadRequest();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(string tableName, [FromBody] GroceryItemEntity entity)
        {
            try
            {
                entity.PartitionKey = entity.Category;

                string Id = Guid.NewGuid().ToString();
                entity.Id = Id;
                entity.RowKey = Id;

                var createdEntity = await _storageService.AddEntityAsync(tableName, entity);

                if (createdEntity == null)
                    return BadRequest();
                return CreatedAtAction(nameof(GetAsync), createdEntity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(string tableName, [FromBody] GroceryItemEntity entity)
        {
            try
            {
                entity.PartitionKey = entity.Category;
                entity.RowKey = entity.Id;

                var response = await _storageService.UpsertEntityAsync(tableName, entity);

                if (response == null) return BadRequest();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string tableName, [FromQuery] string category, string id)
        {
            try
            {
                var response = await _storageService.DeleteEntityAsync(tableName, category, id);
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
