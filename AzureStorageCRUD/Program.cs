using AzureStorageCRUD.Controllers;
using AzureStorageCRUD.Repositories;
using AzureStorageCRUD.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<IBlobStorageRepo, BlobStorageRepo>();
builder.Services.AddScoped<ITableService, TableService>();
builder.Services.AddScoped<IQueueService, QueueService>();
builder.Services.AddScoped<IFileShare, AzureStorageCRUD.Repositories.FileShare>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
