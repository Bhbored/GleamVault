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

            var random = new Random();
            var now = DateTime.Now;

            for (int i = 0; i < 150; i++)
            {
                var daysAgo = random.Next(0, 90);
                var createdAt = now.AddDays(-daysAgo);
                var customer = customers[random.Next(customers.Count)];
                var product = products[random.Next(products.Count)];
                var quantity = random.Next(1, 4);
                var channel = random.Next(2) == 0 ? SaleChannel.InStore : SaleChannel.Online;
                
                TransactionType type;
                string description;
                
                var saleType = random.Next(100);
                if (saleType < 50)
                {
                    type = TransactionType.Sell;
                    description = "Direct Sale";
                }
                else if (saleType < 70)
                {
                    type = TransactionType.CustomeOrder;
                    description = "Bespoke";
                }
                else if (saleType < 80)
                {
                    type = TransactionType.Sell;
                    description = "Gold Booking";
                }
                else if (saleType < 90)
                {
                    type = TransactionType.Repairement;
                    description = "Repair";
                }
                else if (saleType < 95)
                {
                    type = TransactionType.Sell;
                    description = "Gift Card";
                }
                else
                {
                    type = TransactionType.Buy;
                    description = "Appraisal";
                }

                var unitPrice = product.OfferPrice > 0 ? product.OfferPrice : product.UnitPrice;
                var subtotal = unitPrice * quantity;
                var discount = random.Next(0, 50);
                var total = subtotal - discount;

                var transaction = new Transaction
                {
                    CreatedAt = createdAt,
                    Type = type,
                    Channel = channel,
                    CustomerId = customer.Id,
                    Customer = customer,
                    SubTotalAmount = subtotal,
                    DiscountValue = discount,
                    TotalAmount = total,
                    Description = description
                };

                var item = new TransactionItem
                {
                    TransactionId = transaction.Id,
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
                    UnitCost = product.UnitCost
                };

                transaction.Items = new List<TransactionItem> { item };
                transactions.Add(transaction);
            }
        }
    }
}
