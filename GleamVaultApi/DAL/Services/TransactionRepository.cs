using GleamVaultApi.DAL.Contracts;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace GleamVaultApi.DAL.Services
{
    public class TransactionRepository : BaseServiceClass<DB.Transaction>, IViewModelResult<DB.Transaction, Transaction>
    {
        public async Task<List<Transaction>> GetAllAsViewModel()
        {
            using (var db = DatabaseService.GetDB())
            {
                var result = await db.Set<DB.Transaction>()
                    .Include(t => t.TransactionItem)
                        .ThenInclude(ti => ti.Product)
                    .Include(t => t.TransactionItem)
                        .ThenInclude(ti => ti.Category)
                    .Include(t => t.Customer)
                    .Include(t => t.CreatedByUser)
                    .OrderByDescending(t => t.CreatedDate)
                    .ToListAsync();

                return result.Select(MapViewModel).ToList();
            }
        }

        public Task<Transaction> GetAsViewModel(Guid id)
        {
            throw new NotImplementedException();
        }

        public Transaction MapViewModel(DB.Transaction entity)
        {
            return new Transaction
            {
                Id = entity.Id,
                Type = entity.Type,
                TotalAmount = entity.TotalAmount,
                SubTotalAmount = entity.SubTotalAmount,
                DiscountValue = entity.DiscountValue ?? 0f, 
                Channel = entity.Channel,
                Description = entity.Description,
                CreatedDate = entity.CreatedDate,
                CustomerId = entity.CustomerID ?? Guid.Empty, 
                CreatedByUserId = entity.CreatedByUserId,

                
                Items = entity.TransactionItem?.Select(ti => new TransactionItem
                {
                    Id = ti.Id,
                    TransactionId = ti.TransactionId,
                    ProductId = ti.ProductId,
                    Quantity = ti.Quantity,
                    UnitPrice = ti.UnitPriceAtSale,
                    UnitCost = ti.UnitCostAtSale,
                    Name = ti.Name,
                    Sku = ti.Sku,
                    Description = ti.Description,
                    Hallmark = ti.Hallmark,
                    Weight = ti.Weight ?? 0f,
                    WeightUnit = ti.WeightUnit,
                    ImageUrl = ti.ImageUrl,
                    IsUniquePiece = ti.IsUniquePiece,
                    OfferPrice = ti.OfferPrice ?? 0f,
                    CategoryId = ti.CategoryId ?? Guid.Empty, 
                    CreatedDate = ti.CreatedDate
                }).ToList() ?? new List<TransactionItem>()
            };
        }

      
        protected override DB.Transaction Map(DB.Transaction entity, DB.Transaction sourceEnity)
        {
            return null;
        }
    }
}