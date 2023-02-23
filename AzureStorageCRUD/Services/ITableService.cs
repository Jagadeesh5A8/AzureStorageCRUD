using AzureStorageCRUD.Entities;

namespace AzureStorageCRUD.Services
{
    public interface ITableService
    {
        Task<GroceryItemEntity> GetEntityAsync(string category, string id);
        Task<GroceryItemEntity> AddEntityAsync(GroceryItemEntity entity);
        Task<GroceryItemEntity> UpsertEntityAsync(GroceryItemEntity entity);
        Task<bool> DeleteEntityAsync(string category, string id);
    }
}
