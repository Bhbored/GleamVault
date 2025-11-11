using GleamVaultApi.DAL.Contracts;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.Security.Principal;

namespace GleamVaultApi.DAL.Services
{
    public class TransactionRepository : BaseServiceClass<DB.Transaction>, IViewModelResult<DB.Transaction, Transaction>
    {
        public async Task<List<Transaction>> GetAllAsViewModel()
        {
            using (var db = DatabaseService.GetDB())
            {
                var result = await db.Set<DB.Transaction>()
                    .Include(t => t.Customer)
                    .Include(t => t.CreatedByUser)
                    .OrderByDescending(t => t.CreatedDate)
                    .ToListAsync();
                return result.Select(MapViewModelWithoutItems).ToList();
            }
        }

        // Get single transaction without items
        public async Task<Transaction> GetAsViewModel(Guid id)
        {
            using (var db = DatabaseService.GetDB())
            {
                var result = await db.Set<DB.Transaction>()
                    .Include(t => t.Customer)
                    .Include(t => t.CreatedByUser)
                    .FirstOrDefaultAsync(t => t.Id == id);

                return result != null ? MapViewModelWithoutItems(result) : null;
            }
        }

        public async Task<List<TransactionItem>> GetTransactionItems(Guid transactionId)
        {
            using (var db = DatabaseService.GetDB())
            {
                var items = await db.Set<DB.TransactionItem>()
                    .Include(ti => ti.Product)
                    .Include(ti => ti.Category)
                    .Where(ti => ti.TransactionId == transactionId)
                    .OrderBy(ti => ti.CreatedDate)
                    .ToListAsync();

                return items.Select(MapTransactionItemViewModel).ToList();
            }
        }

        // Get complete transaction with items
        public async Task<Transaction> GetTransactionWithItems(Guid id)
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
                    .FirstOrDefaultAsync(t => t.Id == id);

                return result != null ? MapViewModel(result) : null;
            }
        }

        private Transaction MapViewModelWithoutItems(DB.Transaction entity)
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
                Items = new List<TransactionItem>() // Empty items for listing
            };
        }

        // Helper method for transaction items
        private TransactionItem MapTransactionItemViewModel(DB.TransactionItem entity)
        {
            return new TransactionItem
            {
                Id = entity.Id,
                TransactionId = entity.TransactionId,
                ProductId = entity.ProductId,
                Quantity = entity.Quantity,
                UnitPrice = entity.UnitPriceAtSale,
                UnitCost = entity.UnitCostAtSale,
                Name = entity.Name,
                Sku = entity.Sku,
                Description = entity.Description,
                Hallmark = entity.Hallmark,
                Weight = entity.Weight ?? 0f,
                WeightUnit = entity.WeightUnit,
                ImageUrl = entity.ImageUrl,
                IsUniquePiece = entity.IsUniquePiece,
                OfferPrice = entity.OfferPrice ?? 0f,
                CategoryId = entity.CategoryId ?? Guid.Empty,
                CreatedDate = entity.CreatedDate,
             
            };
        }


        public async Task<DB.Transaction> SaveTransaction(Transaction viewModel, IIdentity user)
        {
            using (var db = DatabaseService.GetDB())
            {
               
                var dbTransaction = MapViewModelToDbEntity(viewModel);

               
                if (viewModel.Id != Guid.Empty)
                {
                    var original = await db.Set<DB.Transaction>()
                        .Include(t => t.TransactionItem)
                        .FirstOrDefaultAsync(t => t.Id == viewModel.Id);

                    if (original != null)
                    {
                        var userName = user?.Name ?? "System";
                        var now = DateTime.Now;

                       
                        Map(original, dbTransaction);
                        original.ModifiedBy = userName;
                        original.ModifiedDate = now;

                     
                        var existingItemIds = original.TransactionItem.Select(ti => ti.Id).ToList();
                        var incomingItemIds = viewModel.Items?.Select(ti => ti.Id).Where(id => id != Guid.Empty).ToList() ?? new List<Guid>();

                        
                        var itemsToRemove = original.TransactionItem
                            .Where(ti => !incomingItemIds.Contains(ti.Id))
                            .ToList();

                        foreach (var item in itemsToRemove)
                        {
                            db.Set<DB.TransactionItem>().Remove(item);
                        }

                      
                        if (viewModel.Items != null)
                        {
                            foreach (var itemViewModel in viewModel.Items)
                            {
                                if (itemViewModel.Id != Guid.Empty && existingItemIds.Contains(itemViewModel.Id))
                                {
                                  
                                    var existingItem = original.TransactionItem.First(ti => ti.Id == itemViewModel.Id);
                                    MapTransactionItemViewModelToEntity(itemViewModel, existingItem);
                                    existingItem.ModifiedBy = userName;
                                    existingItem.ModifiedDate = now;
                                }
                                else
                                {
                                    
                                    var newItem = new DB.TransactionItem
                                    {
                                        Id = Guid.NewGuid(),
                                        TransactionId = original.Id,
                                        CreatedBy = userName,
                                        CreatedDate = now,
                                        ModifiedBy = userName,
                                        ModifiedDate = now
                                    };
                                    MapTransactionItemViewModelToEntity(itemViewModel, newItem);
                                    original.TransactionItem.Add(newItem);
                                }
                            }
                        }

                        await db.SaveChangesAsync();
                        return original;
                    }
                }

                
                var userName2 = user?.Name ?? "System";
                var now2 = DateTime.Now;

                dbTransaction.Id = Guid.NewGuid();
                dbTransaction.CreatedBy = userName2;
                dbTransaction.CreatedDate = now2;
                dbTransaction.ModifiedBy = userName2;
                dbTransaction.ModifiedDate = now2;

                if (viewModel.Items != null)
                {
                    dbTransaction.TransactionItem = new List<DB.TransactionItem>();
                    foreach (var itemViewModel in viewModel.Items)
                    {
                        var newItem = new DB.TransactionItem
                        {
                            Id = Guid.NewGuid(),
                            TransactionId = dbTransaction.Id,
                            CreatedBy = userName2,
                            CreatedDate = now2,
                            ModifiedBy = userName2,
                            ModifiedDate = now2
                        };
                        MapTransactionItemViewModelToEntity(itemViewModel, newItem);
                        dbTransaction.TransactionItem.Add(newItem);
                    }
                }

                db.Set<DB.Transaction>().Add(dbTransaction);
                await db.SaveChangesAsync();
                return dbTransaction;
            }
        }

       
        private DB.Transaction MapViewModelToDbEntity(Transaction viewModel)
        {
            return new DB.Transaction
            {
                Id = viewModel.Id,
                Type = viewModel.Type,
                TotalAmount = viewModel.TotalAmount,
                SubTotalAmount = viewModel.SubTotalAmount,
                DiscountValue = viewModel.DiscountValue,
                Channel = viewModel.Channel,
                Description = viewModel.Description,
                CustomerID = viewModel.CustomerId != Guid.Empty ? viewModel.CustomerId : null,
                CreatedByUserId = viewModel.CreatedByUserId
            };
        }

       
        private void MapTransactionItemViewModelToEntity(TransactionItem viewModel, DB.TransactionItem entity)
        {
            entity.ProductId = viewModel.ProductId;
            entity.Quantity = viewModel.Quantity;
            entity.UnitPriceAtSale = viewModel.UnitPrice;
            entity.UnitCostAtSale = viewModel.UnitCost;
            entity.Name = viewModel.Name;
            entity.Sku = viewModel.Sku;
            entity.Description = viewModel.Description;
            entity.Hallmark = viewModel.Hallmark;
            entity.Weight = viewModel.Weight;
            entity.WeightUnit = viewModel.WeightUnit;
            entity.ImageUrl = viewModel.ImageUrl;
            entity.IsUniquePiece = viewModel.IsUniquePiece;
            entity.OfferPrice = viewModel.OfferPrice;
            entity.CategoryId = viewModel.CategoryId != Guid.Empty ? viewModel.CategoryId : null;
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

        
        protected override DB.Transaction Map(DB.Transaction original, DB.Transaction sourceEntity)
        {
            original.Type = sourceEntity.Type;
            original.TotalAmount = sourceEntity.TotalAmount;
            original.SubTotalAmount = sourceEntity.SubTotalAmount;
            original.DiscountValue = sourceEntity.DiscountValue;
            original.Channel = sourceEntity.Channel;
            original.Description = sourceEntity.Description;
            original.CustomerID = sourceEntity.CustomerID;
            original.CreatedByUserId = sourceEntity.CreatedByUserId;
            return original;
        }
    }
}