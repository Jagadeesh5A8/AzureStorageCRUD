using AzureStorageCRUD.Entities;

namespace AzureStorageCRUD.Services
{
    public interface ITableService
    {
        Task<GroceryItemEntity> GetEntityAsync(string tableName, string category, string id);
        Task<GroceryItemEntity> AddEntityAsync(string tableName, GroceryItemEntity entity);
        Task<GroceryItemEntity> UpsertEntityAsync(string tableName, GroceryItemEntity entity);
        Task<bool> DeleteEntityAsync(string tableName, string category, string id);
    }
}
