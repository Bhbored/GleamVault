using GleamVaultApi.DAL.Services;
using GleamVaultApi.DB;
using GleamVaultApi.Extension;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}); 
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddDbContext<GleamVaultContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<CategoryRepository, CategoryRepository>();
builder.Services.AddSingleton<ItemRepository, ItemRepository>();
builder.Services.AddSingleton<CustomerRepository, CustomerRepository>();
builder.Services.AddSingleton<TransactionRepository, TransactionRepository>();



// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseApiKeyValidation();

app.UseAuthorization();

app.MapControllers();

app.Run();
