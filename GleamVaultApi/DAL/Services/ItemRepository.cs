using GleamVaultApi.DAL.Contracts;
using GleamVaultApi.DB;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Models.Enums;
using System.Security.Principal;
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
           
            original.Sku = updated.Sku;
            original.Name = updated.Name;
            original.Description = updated.Description;
            original.Hallmark = updated.Hallmark;
            original.Weight = updated.Weight;
            original.UnitPrice = updated.UnitPrice;
            original.WeightUnit = updated.WeightUnit;
            original.CurrentStock = updated.CurrentStock;
            original.CategoryId = updated.CategoryId;
            original.OfferPrice = updated.OfferPrice;
            original.UnitCost = updated.UnitCost;
            original.IsUniquePiece = updated.IsUniquePiece;


            return original;
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

        public async Task<Shared.Models.Product> SaveAsync(Shared.Models.Product productInfo, IIdentity user)
        {

            if (productInfo == null)
                throw new ArgumentNullException(nameof(productInfo), "Category data is required");

            return await Task.Run(async () =>
            {
                using (var db = DatabaseService.GetDB())
                {
                  

                  
                    var productEntity = new Product()
                    {
                        Id = productInfo.Id,
                        Sku = productInfo.Sku,
                        Name = productInfo.Name,
                        Description = productInfo.Description,
                        Hallmark = productInfo.Hallmark,
                        Weight = productInfo.Weight,
                        WeightUnit = productInfo.WeightUnit,
                        UnitPrice = productInfo.UnitPrice,
                        CurrentStock = productInfo.CurrentStock,
                        CategoryId = productInfo.CategoryId ,
                        OfferPrice=productInfo.OfferPrice,
                        UnitCost=productInfo.UnitCost,    
                        IsUniquePiece=productInfo.IsUniquePiece
                    };

                   
                    var savedEntity = await Update(productEntity, user);

                   
                    return new Shared.Models.Product()
                    {
                        Id = savedEntity.Id,
                        Sku = savedEntity.Sku,
                        Name = savedEntity.Name,
                        Description = savedEntity.Description,
                        Hallmark = savedEntity.Hallmark,
                        Weight = savedEntity.Weight ?? 0f,
                        UnitPrice = savedEntity.UnitPrice,
                        WeightUnit = savedEntity.WeightUnit,
                        CurrentStock = savedEntity.CurrentStock,
                        CategoryId = savedEntity.CategoryId,
                        UnitCost=savedEntity.UnitCost,
                        IsUniquePiece = savedEntity.IsUniquePiece,
                    };
                }
            });
        }
    }
}
