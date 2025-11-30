using PropertyChanged;
using Shared.Models;
using Shared.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GleamVault.TestData
{
    [AddINotifyPropertyChangedInterface]
    public static class TestTransactions
    {
        private static ObservableCollection<Transaction> transactions = new();

        public static ObservableCollection<Transaction> Transactions
        {
            get => transactions;
            set
            {
                transactions = value;
            }
        }

        public static void GenerateTestTransactions(List<Product> products, List<Customer> customers)
        {
            if (transactions.Count > 0) return;

            var now = DateTime.Now;
            var baseDate = now.AddDays(-45);

            if (products == null || products.Count == 0 || customers == null || customers.Count == 0)
                return;

            var productIndex = 0;
            var customerIndex = 0;

            for (int i = 0; i < 20; i++)
            {
                var createdAt = baseDate.AddDays(i * 2);
                var customer = customers[customerIndex % customers.Count];
                var product = products[productIndex % products.Count];
                var channel = i % 2 == 0 ? SaleChannel.InStore : SaleChannel.Online;
                
                var unitPrice = product.OfferPrice > 0 ? product.OfferPrice : product.UnitPrice;
                var quantity = (i % 3) + 1;
                var subtotal = unitPrice * quantity;
                var discount = i % 5 == 0 ? subtotal * 0.1f : 0;
                var total = subtotal - discount;

                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = createdAt,
                    CreatedDate = createdAt,
                    Type = TransactionType.Sell,
                    Channel = channel,
                    CustomerId = customer.Id,
                    Customer = customer,
                    SubTotalAmount = subtotal,
                    DiscountValue = discount,
                    TotalAmount = total,
                    Description = $"Sell Transaction #{i + 1}",
                    CreatedByUserId = Guid.NewGuid(),
                    Items = new List<TransactionItem>
                    {
                        new TransactionItem
                        {
                            TransactionId = Guid.NewGuid(),
                            ProductId = product.Id,
                            Name = product.Name,
                            Quantity = quantity,
                            UnitPrice = unitPrice,
                            Sku = product.Sku,
                            Description = product.Description,
                            CategoryId = product.CategoryId,
                            Category = product.Category,
                            ImageUrl = product.ImageUrl,
                            Hallmark = product.Hallmark,
                            WeightUnit = product.WeightUnit,
                            Weight = product.Weight,
                            OfferPrice = product.OfferPrice,
                            IsUniquePiece = product.IsUniquePiece,
                            UnitCost = product.UnitCost,
                            CreatedDate = createdAt
                        }
                    }
                };
                transactions.Add(transaction);
                productIndex++;
                customerIndex++;
            }

            for (int i = 0; i < 20; i++)
            {
                var createdAt = baseDate.AddDays(i * 2 + 1);
                var customer = customers[customerIndex % customers.Count];
                var product = products[productIndex % products.Count];
                var channel = i % 2 == 0 ? SaleChannel.InStore : SaleChannel.Online;
                
                var unitPrice = product.UnitPrice * 1.5f;
                var quantity = 1;
                var subtotal = unitPrice * quantity;
                var discount = i % 10 == 0 ? subtotal * 0.05f : 0;
                var total = subtotal - discount;

                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = createdAt,
                    CreatedDate = createdAt,
                    Type = TransactionType.CustomeOrder,
                    Channel = channel,
                    CustomerId = customer.Id,
                    Customer = customer,
                    SubTotalAmount = subtotal,
                    DiscountValue = discount,
                    TotalAmount = total,
                    Description = $"Custom Order #{i + 1}",
                    CreatedByUserId = Guid.NewGuid(),
                    Items = new List<TransactionItem>
                    {
                        new TransactionItem
                        {
                            TransactionId = Guid.NewGuid(),
                            ProductId = product.Id,
                            Name = product.Name,
                            Quantity = quantity,
                            UnitPrice = unitPrice,
                            Sku = product.Sku,
                            Description = product.Description,
                            CategoryId = product.CategoryId,
                            Category = product.Category,
                            ImageUrl = product.ImageUrl,
                            Hallmark = product.Hallmark,
                            WeightUnit = product.WeightUnit,
                            Weight = product.Weight,
                            OfferPrice = product.OfferPrice,
                            IsUniquePiece = product.IsUniquePiece,
                            UnitCost = product.UnitCost,
                            CreatedDate = createdAt
                        }
                    }
                };
                transactions.Add(transaction);
                productIndex++;
                customerIndex++;
            }

            for (int i = 0; i < 20; i++)
            {
                var createdAt = baseDate.AddDays(i * 2 + 0.5);
                var customer = customers[customerIndex % customers.Count];
                var product = products[productIndex % products.Count];
                var channel = SaleChannel.InStore;
                
                var unitPrice = 100f + (i * 20f);
                var quantity = 1;
                var subtotal = unitPrice;
                var discount = 0f;
                var total = subtotal;

                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = createdAt,
                    CreatedDate = createdAt,
                    Type = TransactionType.Repairement,
                    Channel = channel,
                    CustomerId = customer.Id,
                    Customer = customer,
                    SubTotalAmount = subtotal,
                    DiscountValue = discount,
                    TotalAmount = total,
                    Description = $"Repair Service #{i + 1}",
                    CreatedByUserId = Guid.NewGuid(),
                    Items = new List<TransactionItem>
                    {
                        new TransactionItem
                        {
                            TransactionId = Guid.NewGuid(),
                            ProductId = product.Id,
                            Name = product.Name,
                            Quantity = quantity,
                            UnitPrice = unitPrice,
                            Sku = product.Sku,
                            Description = product.Description,
                            CategoryId = product.CategoryId,
                            Category = product.Category,
                            ImageUrl = product.ImageUrl,
                            Hallmark = product.Hallmark,
                            WeightUnit = product.WeightUnit,
                            Weight = product.Weight,
                            OfferPrice = product.OfferPrice,
                            IsUniquePiece = product.IsUniquePiece,
                            UnitCost = product.UnitCost,
                            CreatedDate = createdAt
                        }
                    }
                };
                transactions.Add(transaction);
                productIndex++;
                customerIndex++;
            }

            for (int i = 0; i < 20; i++)
            {
                var createdAt = baseDate.AddDays(i * 2 + 1.5);
                var customer = customers[customerIndex % customers.Count];
                var product = products[productIndex % products.Count];
                var channel = SaleChannel.InStore;
                
                var unitPrice = product.UnitPrice * 0.6f;
                var quantity = 1;
                var subtotal = unitPrice * quantity;
                var discount = 0f;
                var total = subtotal;

                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = createdAt,
                    CreatedDate = createdAt,
                    Type = TransactionType.Buy,
                    Channel = channel,
                    CustomerId = customer.Id,
                    Customer = customer,
                    SubTotalAmount = subtotal,
                    DiscountValue = discount,
                    TotalAmount = total,
                    Description = $"Buy Transaction #{i + 1}",
                    CreatedByUserId = Guid.NewGuid(),
                    Items = new List<TransactionItem>
                    {
                        new TransactionItem
                        {
                            TransactionId = Guid.NewGuid(),
                            ProductId = product.Id,
                            Name = product.Name,
                            Quantity = quantity,
                            UnitPrice = unitPrice,
                            Sku = product.Sku,
                            Description = product.Description,
                            CategoryId = product.CategoryId,
                            Category = product.Category,
                            ImageUrl = product.ImageUrl,
                            Hallmark = product.Hallmark,
                            WeightUnit = product.WeightUnit,
                            Weight = product.Weight,
                            OfferPrice = product.OfferPrice,
                            IsUniquePiece = product.IsUniquePiece,
                            UnitCost = product.UnitCost,
                            CreatedDate = createdAt
                        }
                    }
                };
                transactions.Add(transaction);
                productIndex++;
                customerIndex++;
            }
        }
    }
}
