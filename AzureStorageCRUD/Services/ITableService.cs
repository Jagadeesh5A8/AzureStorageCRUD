using AzureStorageCRUD.Entities;

namespace AzureStorageCRUD.Services
{
    public interface ITableService
    {
        Task<GroceryItemEntity> GetEntityAsync(string category, string id, string tableName);
        Task<GroceryItemEntity> AddEntityAsync(GroceryItemEntity entity, string tableName);
        Task<GroceryItemEntity> UpsertEntityAsync(GroceryItemEntity entity, string tableName);
        Task<bool> DeleteEntityAsync(string category, string id, string tableName);
    }
}
