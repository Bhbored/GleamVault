using GleamVaultApi.DAL.Contracts;
using GleamVaultApi.DB;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Product = GleamVaultApi.DB.Product;

namespace GleamVaultApi.DAL.Services
{
    public class ItemRepository : BaseServiceClass<Product>, IViewModelResult<Product, ItemInfo>
    {
        public Task<List<ItemInfo>> GetAllAsViewModel()
        {
            throw new NotImplementedException();
        }

        public Task<ItemInfo> GetAsViewModel(Guid id)
        {
            throw new NotImplementedException();
        }
        public Task<List<ItemInfo>> GetByCategoryAsViewModel(Guid itemCategoryId)
        {
            return Task.Run(() =>
            {
                using (var db = DatabaseService.GetDB())
                {
                   
                    var result = db.Product
                                  .Where(p => p.CategoryId == itemCategoryId && p.CurrentStock > 0)
                                  .Include(p => p.Category) 
                                  .ToList();
                    return result.Select(s => MapViewModel(s)).ToList();
                }
            });


        }
        protected override Product Map(Product original, Product updated)
        {
            return null;
        }
        public ItemInfo MapViewModel(Product entity)
        {
            if (entity == null) return null;

            return new ItemInfo()
            {
                Id = entity.Id,
                Sku=entity.Sku,
                Name=entity.Name,
                Description=entity.Description,
                HallMark=entity.Hallmark.ToString(),
                weight=entity.Weight ?? 0f,
                UnitPrice=entity.UnitPrice,
                weightUnit=entity.WeightUnit.ToString(),
                CurrentStock=entity.CurrentStock

            };
        }
    }
}
