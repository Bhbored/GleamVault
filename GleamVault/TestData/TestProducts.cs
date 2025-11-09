using Bogus;
using PropertyChanged;
using Shared.Models;
using Shared.Models.Enums;
using System.Collections.ObjectModel;

namespace GleamVault.TestData
{
    [AddINotifyPropertyChangedInterface]

    public class TestProducts
    {
        private ObservableCollection<Category> categories = new ObservableCollection<Category>(GetCategories());



        public static ObservableCollection<Product> Products { get; set; } = new()
        {
             new Product
        {
            Id = Guid.NewGuid(),
            Sku = "RG001",
            Name = "Classic Gold Wedding Band",
            Description = "Elegant 18K gold wedding band with polished finish",
            CategoryId = Guid.NewGuid(),
            UnitCost = 450f,
            UnitPrice = 899f,
            OfferPrice = 799f,
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
            CategoryId = Guid.NewGuid(),
            UnitCost = 1200f,
            UnitPrice = 2500f,
            CurrentStock = 8,
            Hallmark = HallmarkType.Sterling925,
            WeightUnit = WeightUnit.Carats,
            Weight = 1.0f
        },

        };
        public ObservableCollection<Category> Categories
        {
            get => categories;
            set
            {
                categories = value;
            }
        }
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
            OfferPrice = 799f,
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

        public static List<Customer> GetCustomers()
        {
            var testCustomers = new List<Customer>
{
    new Customer
    {
        Id = Guid.NewGuid(),
        FullName = "John Smith",
        PhoneNumber = "+1-555-0101",
        Email = "john.smith@email.com",
        Address = "123 Main St, New York, NY 10001",
        DateOfBirth = new DateTime(1985, 3, 15),
        Notes = "Preferred customer, likes email communication",
        LoyaltyPoints = 150
    },
    new Customer
    {
        Id = Guid.NewGuid(),
        FullName = "Sarah Johnson",
        PhoneNumber = "+1-555-0102",
        Email = "sarah.j@email.com",
        Address = "456 Oak Avenue, Los Angeles, CA 90210",
        DateOfBirth = new DateTime(1990, 7, 22),
        Notes = "Frequent buyer, prefers text messages",
        LoyaltyPoints = 275
    },
    new Customer
    {
        Id = Guid.NewGuid(),
        FullName = "Michael Chen",
        PhoneNumber = "+1-555-0103",
        Email = "michael.chen@email.com",
        Address = "789 Pine Road, Chicago, IL 60616",
        DateOfBirth = new DateTime(1982, 11, 8),
        Notes = "VIP customer, handle with care",
        LoyaltyPoints = 500
    },
    new Customer
    {
        Id = Guid.NewGuid(),
        FullName = "Emily Davis",
        PhoneNumber = "+1-555-0104",
        Email = null, // Customer prefers not to share email
        Address = "321 Elm Street, Miami, FL 33101",
        DateOfBirth = null, // Date of birth not provided
        Notes = "New customer",
        LoyaltyPoints = 25
    },
    new Customer
    {
        Id = Guid.NewGuid(),
        FullName = "Robert Wilson",
        PhoneNumber = null, // No phone number provided
        Email = "robert.wilson@email.com",
        Address = "654 Maple Drive, Seattle, WA 98101",
        DateOfBirth = new DateTime(1978, 12, 3),
        Notes = "Corporate account",
        LoyaltyPoints = 180
    },
    new Customer
    {
        Id = Guid.NewGuid(),
        FullName = "Lisa Martinez",
        PhoneNumber = "+1-555-0106",
        Email = "lisa.martinez@email.com",
        Address = null, // Address not provided
        DateOfBirth = new DateTime(1995, 5, 18),
        Notes = "Student discount applied",
        LoyaltyPoints = 75
    },
    new Customer
    {
        Id = Guid.NewGuid(),
        FullName = "David Brown",
        PhoneNumber = "+1-555-0107",
        Email = "david.brown@email.com",
        Address = "987 Cedar Lane, Boston, MA 02108",
        DateOfBirth = new DateTime(1988, 9, 30),
        Notes = "Returns items frequently",
        LoyaltyPoints = 45
    },
    new Customer
    {
        Id = Guid.NewGuid(),
        FullName = "Jennifer Lee",
        PhoneNumber = "+1-555-0108",
        Email = "jennifer.lee@email.com",
        Address = "147 Walnut Avenue, San Francisco, CA 94102",
        DateOfBirth = new DateTime(1992, 2, 14),
        Notes = "Loyal customer for 5 years",
        LoyaltyPoints = 650
    },
    new Customer
    {
        Id = Guid.NewGuid(),
        FullName = "Thomas Anderson",
        PhoneNumber = "+1-555-0109",
        Email = "thomas.a@email.com",
        Address = "258 Spruce Court, Austin, TX 73301",
        DateOfBirth = null,
        Notes = "Bulk order customer",
        LoyaltyPoints = 320
    },
    new Customer
    {
        Id = Guid.NewGuid(),
        FullName = "Amanda Taylor",
        PhoneNumber = null,
        Email = null,
        Address = "369 Birch Road, Denver, CO 80202",
        DateOfBirth = new DateTime(1987, 6, 9),
        Notes = "Minimal contact information provided",
        LoyaltyPoints = 10
    }
};

            return testCustomers;
        }



    }
}
