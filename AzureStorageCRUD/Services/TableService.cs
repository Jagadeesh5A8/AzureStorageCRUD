﻿using Azure.Data.Tables;
using AzureStorageCRUD.Entities;

namespace AzureStorageCRUD.Services
{
    public class TableService:ITableService
    {
        private readonly IConfiguration _configuration;
       // private const string tableName = "entity";
        public TableService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<GroceryItemEntity> GetEntityAsync(string category, string id, string tableName)
        {
            var tableClient = await GetTableClient( tableName);

            var entity = await tableClient.GetEntityAsync<GroceryItemEntity>(category, id);

            return entity;
        }
        public async Task<GroceryItemEntity> AddEntityAsync(GroceryItemEntity entity, string tableName)
        {
            var tableClient = await GetTableClient( tableName);

            await tableClient.AddEntityAsync(entity);

            return entity;
        }
        public async Task<GroceryItemEntity> UpsertEntityAsync(GroceryItemEntity entity, string tableName)
        {
            var tableClient = await GetTableClient( tableName);

            await tableClient.UpsertEntityAsync(entity);

            return entity;
        }
        public async Task<bool> DeleteEntityAsync(string category, string id, string tableName)
        {
            var tableClient = await GetTableClient( tableName);

            var result = await tableClient.DeleteEntityAsync(category, id);

            if(result.IsError == false)
            {
                return true;
            }

            return false;
        }

        private async Task<TableClient> GetTableClient(string tableName)
        {
            var serviceClient = new TableServiceClient(_configuration["StorageConnectionString"]);

            var tableClient = serviceClient.GetTableClient(tableName);

            await tableClient.CreateIfNotExistsAsync();

            return tableClient;
        }
    }
}
