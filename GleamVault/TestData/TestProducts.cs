using Bogus;
using Shared.Models;
using Shared.Models.Enums;

namespace GleamVault.TestData
{
    public class TestProducts
    {

        public static List<Product> GetProducts()
        {
            var categories = new List<Category>
    {
        new Category { Id = Guid.NewGuid(), Name = "Rings", Description = "Beautiful rings for every occasion" },
        new Category { Id = Guid.NewGuid(), Name = "Necklaces", Description = "Elegant necklaces and pendants" },
        new Category { Id = Guid.NewGuid(), Name = "Earrings", Description = "Stunning earrings in various styles" },
        new Category { Id = Guid.NewGuid(), Name = "Bracelets", Description = "Charming bracelets and bangles" },
        new Category { Id = Guid.NewGuid(), Name = "Watches", Description = "Luxury timepieces" },
        new Category { Id = Guid.NewGuid(), Name = "Brooches", Description = "Elegant brooches and pins" },
        new Category { Id = Guid.NewGuid(), Name = "Cufflinks", Description = "Stylish cufflinks for formal wear" },
        new Category { Id = Guid.NewGuid(), Name = "Anklets", Description = "Delicate ankle bracelets" },
        new Category { Id = Guid.NewGuid(), Name = "Charms", Description = "Charm bracelets and individual charms" },
        new Category { Id = Guid.NewGuid(), Name = "Bridal", Description = "Wedding and engagement jewelry" }
    };

            var products = new List<Product>
    {
        new Product
        {
            Id = Guid.NewGuid(),
            Sku = "RG001",
            Name = "Classic Gold Wedding Band",
            Description = "Elegant 18K gold wedding band with polished finish",
            CategoryId = categories[0].Id,
            Category = categories[0],
            UnitCost = 450f,
            UnitPrice = 899f,
            CurrentStock = 15,
            Hallmark = HallmarkType.Gold18K,
            WeightUnit = WeightUnit.Grams,
            Weight = 4.2f
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Sku = "RG002",
            Name = "Diamond Engagement Ring",
            Description = "Stunning 1-carat solitaire diamond ring in platinum setting",
            CategoryId = categories[0].Id,
            Category = categories[0],
            UnitCost = 1200f,
            UnitPrice = 2500f,
            CurrentStock = 8,
            Hallmark = HallmarkType.Sterling925,
            WeightUnit = WeightUnit.Carats,
            Weight = 1.0f
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Sku = "NK001",
            Name = "Pearl Strand Necklace",
            Description = "Luxurious 18-inch cultured pearl necklace",
            CategoryId = categories[1].Id,
            Category = categories[1],
            UnitCost = 300f,
            UnitPrice = 650f,
            CurrentStock = 12,
            Hallmark = HallmarkType.LuxuryBrands,
            WeightUnit = WeightUnit.Grams,
            Weight = 22.5f
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Sku = "NK002",
            Name = "Gold Chain with Pendant",
            Description = "14K gold chain with diamond-cut cross pendant",
            CategoryId = categories[1].Id,
            Category = categories[1],
            UnitCost = 280f,
            UnitPrice = 550f,
            CurrentStock = 10,
            Hallmark = HallmarkType.Gold14K,
            WeightUnit = WeightUnit.Grams,
            Weight = 8.7f
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Sku = "ER001",
            Name = "Diamond Stud Earrings",
            Description = "Brilliant 0.5-carat diamond stud earrings in white gold",
            CategoryId = categories[2].Id,
            Category = categories[2],
            UnitCost = 600f,
            UnitPrice = 1200f,
            CurrentStock = 6,
            Hallmark = HallmarkType.Sterling925,
            WeightUnit = WeightUnit.Carats,
            Weight = 1.0f
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Sku = "ER002",
            Name = "Gold Hoop Earrings",
            Description = "Classic 10K gold hoop earrings, 20mm diameter",
            CategoryId = categories[2].Id,
            Category = categories[2],
            UnitCost = 150f,
            UnitPrice = 299f,
            CurrentStock = 20,
            Hallmark = HallmarkType.Gold10K,
            WeightUnit = WeightUnit.Grams,
            Weight = 3.8f
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Sku = "BR001",
            Name = "Tennis Bracelet",
            Description = "Elegant diamond tennis bracelet with 2 carats total weight",
            CategoryId = categories[3].Id,
            Category = categories[3],
            UnitCost = 800f,
            UnitPrice = 1600f,
            CurrentStock = 5,
            Hallmark = HallmarkType.Sterling925,
            WeightUnit = WeightUnit.Carats,
            Weight = 2.0f
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Sku = "BR002",
            Name = "Gold Bangle Set",
            Description = "Set of three 21K gold bangles in different widths",
            CategoryId = categories[3].Id,
            Category = categories[3],
            UnitCost = 420f,
            UnitPrice = 850f,
            CurrentStock = 8,
            Hallmark = HallmarkType.Gold21K,
            WeightUnit = WeightUnit.Grams,
            Weight = 15.3f
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Sku = "WT001",
            Name = "Luxury Swiss Watch",
            Description = "Automatic mechanical watch with leather strap",
            CategoryId = categories[4].Id,
            Category = categories[4],
            UnitCost = 1500f,
            UnitPrice = 3200f,
            CurrentStock = 3,
            Hallmark = HallmarkType.LuxuryBrands,
            WeightUnit = WeightUnit.Grams,
            Weight = 85.0f,
            IsUniquePiece = true
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Sku = "WT002",
            Name = "Gold Dress Watch",
            Description = "Elegant 18K gold dress watch with mother-of-pearl dial",
            CategoryId = categories[4].Id,
            Category = categories[4],
            UnitCost = 900f,
            UnitPrice = 1800f,
            CurrentStock = 4,
            Hallmark = HallmarkType.Gold18K,
            WeightUnit = WeightUnit.Grams,
            Weight = 62.5f
        }
    };

            return products;
        }

        public static List<Category> GetCategories()
        {
            var categories = new List<Category>
            {
                new Category { Id = Guid.NewGuid(), Name = "Rings", Description = "Beautiful rings for every occasion" },
                new Category { Id = Guid.NewGuid(), Name = "Necklaces", Description = "Elegant necklaces and pendants" },
                new Category { Id = Guid.NewGuid(), Name = "Earrings", Description = "Stunning earrings in various styles" },
                new Category { Id = Guid.NewGuid(), Name = "Bracelets", Description = "Charming bracelets and bangles" },
                new Category { Id = Guid.NewGuid(), Name = "Watches", Description = "Luxury timepieces" },
                new Category { Id = Guid.NewGuid(), Name = "Brooches", Description = "Elegant brooches and pins" },
                new Category { Id = Guid.NewGuid(), Name = "Cufflinks", Description = "Stylish cufflinks for formal wear" },
                new Category { Id = Guid.NewGuid(), Name = "Anklets", Description = "Delicate ankle bracelets" },
                new Category { Id = Guid.NewGuid(), Name = "Charms", Description = "Charm bracelets and individual charms" },
                new Category { Id = Guid.NewGuid(), Name = "Bridal", Description = "Wedding and engagement jewelry" }
            };
            return categories;
        }
    }
}
