using Bogus;
using GleamVault.MVVM.ViewModels;
using PropertyChanged;
using Shared.Models;
using Shared.Models.Enums;
using System.Collections.ObjectModel;

namespace GleamVault.TestData
{
    [AddINotifyPropertyChangedInterface]

    public class TestProducts
    {
        public static List<Product> GetProducts()
        {

            var products = new List<Product>
{
    new Product
    {
        Id = Guid.Parse("1e65bbfb-aa20-4984-973b-0cdaddf2c2bf"),
        Sku = "WATCH-002",
        Name = "Mens Gold Watch",
        Description = "Automatic movement, gold case and bracelet",
        CategoryId = Guid.Parse("4efd3c63-b7de-4fa6-9a62-f76b86042a6e"),
        UnitCost = 4200,
        Hallmark = HallmarkType.Gold9K,
        WeightUnit = WeightUnit.Grams,
        Weight = 125,
        UnitPrice = 7800,
        CurrentStock = 1,
        IsUniquePiece = true,
        ImageUrl = "gold_watch_for_man.jpg"
    },
    new Product
    {
        Id = Guid.Parse("7a5521fe-91fe-483c-bb7b-0d7a3f9a550b"),
        Sku = "CUFF-001",
        Name = "Diamond Cufflinks",
        Description = "Square cufflinks with diamond centers",
        CategoryId = Guid.Parse("37343a37-6d90-4def-b8cb-bd56f225a753"),
        UnitCost = 420,
        Hallmark = HallmarkType.Gold9K,
        WeightUnit = WeightUnit.Grams,
        Weight = 6.5f,
        UnitPrice = 800,
        CurrentStock = 5,
        IsUniquePiece = false,
        ImageUrl = "cuff001a.jpg"
    },
    new Product
    {
        Id = Guid.Parse("3b0dc7f4-baff-47b8-89ac-0e7992ee9402"),
        Sku = "RING-003",
        Name = "Rose Gold Wedding Band",
        Description = "Classic plain wedding band in rose gold",
        CategoryId = Guid.Parse("b5c9f4e1-209c-45ab-a7ab-9cd24791ee1d"),
        UnitCost = 350,
        Hallmark = HallmarkType.Gold9K,
        WeightUnit = WeightUnit.Grams,
        Weight = 5,
        UnitPrice = 650,
        CurrentStock = 12,
        IsUniquePiece = false,
        ImageUrl = "ring003a.jpg"
    },
    new Product
    {
        Id = Guid.Parse("e54b8785-bf9d-4d4b-9fff-0f5995086469"),
        Sku = "GV-DIAMOND-001",
        Name = "Premium Diamond Stud Earrings",
        Description = "Exclusive 1-carat diamond earrings",
        CategoryId = Guid.Parse("b3905666-d8a4-47f3-8413-e8e5c907b0e3"),
        UnitCost = 2500,
        Hallmark = HallmarkType.Gold18K,
        WeightUnit = WeightUnit.Carats,
        Weight = 1,
        UnitPrice = 4499.99f,
        OfferPrice = 0,
        CurrentStock = 1,
        IsUniquePiece = true,
        ImageUrl = "diamond_studs.jpeg"
    },
    new Product
    {
        Id = Guid.Parse("09b81520-ead0-43a4-9bce-14c94c6e9678"),
        Sku = "NECK-001",
        Name = "Pearl Strand Necklace",
        Description = "Cultured pearls, 18 inch strand",
        CategoryId = Guid.Parse("1ef1f3e6-e894-48b0-9fce-7d80d67b4d8e"),
        UnitCost = 800,
        Hallmark = HallmarkType.Gold9K,
        WeightUnit = WeightUnit.Grams,
        Weight = 25,
        UnitPrice = 1400,
        CurrentStock = 8,
        IsUniquePiece = false,
        ImageUrl = "neck001a.jpg"
    },
    new Product
    {
        Id = Guid.Parse("3af227e6-d415-4d08-9e5a-14e0b632fc07"),
        Sku = "BRAC-001",
        Name = "Tennis Bracelet",
        Description = "Classic diamond tennis bracelet",
        CategoryId = Guid.Parse("819a8a81-0a8d-4fc2-8321-6d499e513517"),
        UnitCost = 1800,
        Hallmark = HallmarkType.Gold14K,
        WeightUnit = WeightUnit.Grams,
        Weight = 12,
        UnitPrice = 3200,
        CurrentStock = 6,
        IsUniquePiece = false,
        ImageUrl = "brac001a.jpg"
    },
    new Product
    {
        Id = Guid.Parse("07edcbdb-fdb1-4634-8e9a-1719a254d5ba"),
        Sku = "ANK-001",
        Name = "Gold Anklet",
        Description = "Delicate chain anklet with heart charm",
        CategoryId = Guid.Parse("31aa9977-f86d-41ac-b943-7dbfadf3f39f"),
        UnitCost = 180,
        Hallmark = HallmarkType.Gold14K,
        WeightUnit = WeightUnit.Grams,
        Weight = 2.8f,
        UnitPrice = 350,
        OfferPrice = 299,
        CurrentStock = 10,
        IsUniquePiece = false,
        ImageUrl = "ank001a.jpeg"
    },
    new Product
    {
        Id = Guid.Parse("4aad6d3a-c007-4fac-9955-22dfe315b13b"),
        Sku = "GV-GOLD-001",
        Name = "18K Gold Diamond Ring",
        Description = "Beautiful 18K gold ring with premium diamond",
        CategoryId = Guid.Parse("31aa9977-f86d-41ac-b943-7dbfadf3f39f"),
        UnitCost = 0,
        Hallmark = HallmarkType.Gold18K,
        WeightUnit = WeightUnit.Grams,
        Weight = 5.2f,
        UnitPrice = 1250.99f,
        CurrentStock = 10,
        IsUniquePiece = false,
        ImageUrl = "gold_princess_cut_diamond_ring.jpg"
    },
    new Product
    {
        Id = Guid.Parse("134fe9ab-76cd-47df-b8e2-45c7d379dc95"),
        Sku = "RING-001",
        Name = "Diamond Solitaire Engagement Ring",
        Description = "1.5 carat brilliant cut diamond, 14K white gold setting",
        CategoryId = Guid.Parse("b5c9f4e1-209c-45ab-a7ab-9cd24791ee1d"),
        UnitCost = 2500,
        Hallmark = HallmarkType.Gold14K,
        WeightUnit = WeightUnit.Grams,
        Weight = 3.5f,
        UnitPrice = 4500,
        CurrentStock = 5,
        IsUniquePiece = false,
        ImageUrl = "ring001a.jpg"
    },
    new Product
    {
        Id = Guid.Parse("85632460-d890-4d79-9498-4f7cfde80c9b"),
        Sku = "BRID-001",
        Name = "Bridal Set",
        Description = "Engagement ring and wedding band set",
        CategoryId = Guid.Parse("0948a137-7c69-44a0-801c-bbeed85e53d9"),
        UnitCost = 3200,
        Hallmark = HallmarkType.Gold14K,
        WeightUnit = WeightUnit.Grams,
        Weight = 7.5f,
        UnitPrice = 5800,
        CurrentStock = 3,
        IsUniquePiece = false,
        ImageUrl = "brid001a.jpg"
    },
    new Product
    {
        Id = Guid.Parse("a5b06338-2de8-4442-b20f-5d1fcbc4b12c"),
        Sku = "WATCH-001",
        Name = "Ladies Diamond Watch",
        Description = "Swiss movement with diamond bezel",
        CategoryId = Guid.Parse("4efd3c63-b7de-4fa6-9a62-f76b86042a6e"),
        UnitCost = 3500,
        Hallmark = HallmarkType.Sterling925,
        WeightUnit = WeightUnit.Grams,
        Weight = 85,
        UnitPrice = 6500,
        CurrentStock = 2,
        IsUniquePiece = false,
        ImageUrl = "watch001a.jpg"
    },
    new Product
    {
        Id = Guid.Parse("0466d2cf-2409-4d5b-ac32-5d885a3116cf"),
        Sku = "BRAC-003",
        Name = "Charm Bracelet",
        Description = "Silver charm bracelet with starter charms",
        CategoryId = Guid.Parse("819a8a81-0a8d-4fc2-8321-6d499e513517"),
        UnitCost = 180,
        Hallmark = HallmarkType.Sterling925,
        WeightUnit = WeightUnit.Grams,
        Weight = 22,
        UnitPrice = 350,
        CurrentStock = 15,
        IsUniquePiece = false,
        ImageUrl = "brac003a.jpeg"
    },
    new Product
    {
        Id = Guid.Parse("fe89aeaa-d468-4a03-a2e5-600ce0124f75"),
        Sku = "BRAC-002",
        Name = "Gold Bangle",
        Description = "Solid gold bangle with engraving",
        CategoryId = Guid.Parse("819a8a81-0a8d-4fc2-8321-6d499e513517"),
        UnitCost = 650,
        Hallmark = HallmarkType.Sterling925,
        WeightUnit = WeightUnit.Grams,
        Weight = 18.5f,
        UnitPrice = 1200,
        OfferPrice = 1050,
        CurrentStock = 8,
        IsUniquePiece = false,
        ImageUrl = "brac002a.jpeg"
    },
    new Product
    {
        Id = Guid.Parse("d7a949b7-09ca-48fe-b519-7503aa872bc8"),
        Sku = "GV-DIAMOND-004",
        Name = "Premium Diamond Stud Earrings",
        Description = "Exclusive 1-carat diamond earrings",
        CategoryId = Guid.Parse("b3905666-d8a4-47f3-8413-e8e5c907b0e3"),
        UnitCost = 450,
        Hallmark = HallmarkType.Gold18K,
        WeightUnit = WeightUnit.Carats,
        Weight = 1,
        UnitPrice = 4499.99f,
        CurrentStock = 1,
        IsUniquePiece = false,
        ImageUrl = "neck003a.jpeg"
    },
    new Product
    {
        Id = Guid.Parse("a4745811-a176-4908-b0d3-7b6d0a0fec9c"),
        Sku = "EAR-004",
        Name = "Emerald Chandelier Earrings",
        Description = "Statement chandelier earrings with emeralds",
        CategoryId = Guid.Parse("7ebe85e8-6b91-40c9-a32f-e626f02f36a9"),
        UnitCost = 1400,
        Hallmark = HallmarkType.Sterling925,
        WeightUnit = WeightUnit.Grams,
        Weight = 7.5f,
        UnitPrice = 2500,
        CurrentStock = 3,
        IsUniquePiece = false,
        ImageUrl = "ear004a.jpg"
    },
    new Product
    {
        Id = Guid.Parse("98b4c2f2-e1ff-469c-b455-85529f164b43"),
        Sku = "GV-DIAMOND-6673",
        Name = "Luxury Diamond Earrings",
        Description = "Stunning 0.75-carat diamond earrings in platinum setting",
        CategoryId = Guid.Parse("b3905666-d8a4-47f3-8413-e8e5c907b0e3"),
        UnitCost = 777,
        Hallmark = HallmarkType.Gold21K,
        WeightUnit = WeightUnit.Carats,
        Weight = 0.75f,
        UnitPrice = 777.99f,
        OfferPrice = 7777.99f,
        CurrentStock = 3,
        IsUniquePiece = true,
        ImageUrl = "ear0011a.jpg"
    },
    new Product
    {
        Id = Guid.Parse("abbd2a1a-2898-4439-99a1-937e6f4b3430"),
        Sku = "hv-gold-6673",
        Name = "Luxury Diamond Earrings",
        Description = "Stunning 0.75-carat diamond earrings in platinum setting",
        CategoryId = Guid.Parse("b3905666-d8a4-47f3-8413-e8e5c907b0e3"),
        UnitCost = 777,
        Hallmark = HallmarkType.Gold21K,
        WeightUnit = WeightUnit.Carats,
        Weight = 0.75f,
        UnitPrice = 777.99f,
        OfferPrice = 7777.99f,
        CurrentStock = 3,
        IsUniquePiece = true,
        ImageUrl = "ear0012a.jpg"
    },
    new Product
    {
        Id = Guid.Parse("d7bc2d64-b230-4434-bcd5-9aab87844aab"),
        Sku = "NECK-004",
        Name = "Sapphire Drop Pendant",
        Description = "Teardrop blue sapphire with diamond accents",
        CategoryId = Guid.Parse("1ef1f3e6-e894-48b0-9fce-7d80d67b4d8e"),
        UnitCost = 1600,
        Hallmark = HallmarkType.Sterling925,
        WeightUnit = WeightUnit.Grams,
        Weight = 6.2f,
        UnitPrice = 2800,
        CurrentStock = 2,
        IsUniquePiece = false,
        ImageUrl = "neck004a.jpg"
    },
    new Product
    {
        Id = Guid.Parse("1e7b9d2d-b0a6-495c-bfc7-9c8d94f5d376"),
        Sku = "EAR-003",
        Name = "Hoop Earrings",
        Description = "Classic gold hoops, medium size",
        CategoryId = Guid.Parse("7ebe85e8-6b91-40c9-a32f-e626f02f36a9"),
        UnitCost = 320,
        Hallmark = HallmarkType.Gold18K,
        WeightUnit = WeightUnit.Grams,
        Weight = 4.8f,
        UnitPrice = 600,
        CurrentStock = 18,
        IsUniquePiece = false,
        ImageUrl = "ear003a.jpg"
    },
    new Product
    {
        Id = Guid.Parse("49ace86a-1e1a-430d-a822-b1698deea191"),
        Sku = "RING-005",
        Name = "Ruby Eternity Band",
        Description = "Full circle ruby eternity ring",
        CategoryId = Guid.Parse("b5c9f4e1-209c-45ab-a7ab-9cd24791ee1d"),
        UnitCost = 1500,
        Hallmark = HallmarkType.Gold18K,
        WeightUnit = WeightUnit.Grams,
        Weight = 3.8f,
        UnitPrice = 2600,
        CurrentStock = 6,
        IsUniquePiece = false,
        ImageUrl = "ring005a.jpg"
    },
    new Product
    {
        Id = Guid.Parse("3d4733be-c107-478d-a198-b26477739473"),
        Sku = "NECK-002",
        Name = "Diamond Pendant Necklace",
        Description = "Heart-shaped diamond pendant on 14K gold chain",
        CategoryId = Guid.Parse("1ef1f3e6-e894-48b0-9fce-7d80d67b4d8e"),
        UnitCost = 1200,
        Hallmark = HallmarkType.Gold18K,
        WeightUnit = WeightUnit.Grams,
        Weight = 8.5f,
        UnitPrice = 2100,
        OfferPrice = 1850,
        CurrentStock = 4,
        IsUniquePiece = false,
        ImageUrl = "neck002a.jpeg"
    },
    new Product
    {
        Id = Guid.Parse("69bf788e-fcdd-4401-a42f-c44ed0c5f687"),
        Sku = "BRO-001",
        Name = "Vintage Floral Brooch",
        Description = "Art deco style brooch with sapphires",
        CategoryId = Guid.Parse("bc5ca235-21c0-45b7-ad1f-c87c134cb9b5"),
        UnitCost = 850,
        Hallmark = HallmarkType.Gold18K,
        WeightUnit = WeightUnit.Grams,
        Weight = 8.2f,
        UnitPrice = 1600,
        CurrentStock = 1,
        IsUniquePiece = true,
        ImageUrl = "bro001a.jpg"
    },
    new Product
    {
        Id = Guid.Parse("ba4fefdd-b1c4-4904-8e6f-c85cfbc8cb87"),
        Sku = "GV-DIAMOND-333",
        Name = "Luxury Diamond Earrings",
        Description = "Stunning 0.75-carat diamond earrings in platinum setting",
        CategoryId = Guid.Parse("b3905666-d8a4-47f3-8413-e8e5c907b0e3"),
        UnitCost = 3200,
        Hallmark = HallmarkType.Gold21K,
        WeightUnit = WeightUnit.Carats,
        Weight = 0.75f,
        UnitPrice = 4599.99f,
        OfferPrice = 4199.99f,
        CurrentStock = 3,
        IsUniquePiece = true,
        ImageUrl = "earrings0013a.jpg"
    },
    new Product
    {
        Id = Guid.Parse("11a0f2a2-0b06-40ee-b802-cb9c508b3cb3"),
        Sku = "CHARM-001",
        Name = "Heart Charm",
        Description = "Individual heart charm for bracelets",
        CategoryId = Guid.Parse("b3905666-d8a4-47f3-8413-e8e5c907b0e3"),
        UnitCost = 45,
        Hallmark = HallmarkType.Gold18K,
        WeightUnit = WeightUnit.Grams,
        Weight = 1.2f,
        UnitPrice = 95,
        CurrentStock = 25,
        IsUniquePiece = false,
        ImageUrl = "charm001a.jpg"
    },
    new Product
    {
        Id = Guid.Parse("fdc36c56-e4c6-43c8-9278-dab2a4015d32"),
        Sku = "EAR-002",
        Name = "Pearl Drop Earrings",
        Description = "Freshwater pearl drops with gold posts",
        CategoryId = Guid.Parse("7ebe85e8-6b91-40c9-a32f-e626f02f36a9"),
        UnitCost = 280,
        Hallmark = HallmarkType.Gold18K,
        WeightUnit = WeightUnit.Grams,
        Weight = 3.2f,
        UnitPrice = 520,
        OfferPrice = 450,
        CurrentStock = 12,
        IsUniquePiece = false,
        ImageUrl = "ear002a.jpg"
    },
    new Product
    {
        Id = Guid.Parse("d9276644-21ee-4501-8a77-e32d7e0abf0e"),
        Sku = "EAR-001",
        Name = "Diamond Stud Earrings",
        Description = "0.5 carat total weight diamond studs",
        CategoryId = Guid.Parse("7ebe85e8-6b91-40c9-a32f-e626f02f36a9"),
        UnitCost = 600,
        Hallmark = HallmarkType.Gold14K,
        WeightUnit = WeightUnit.Grams,
        Weight = 1.5f,
        UnitPrice = 1200,
        CurrentStock = 10,
        IsUniquePiece = false,
        ImageUrl = "ear001a.jpg"
    },
    new Product
    {
        Id = Guid.Parse("b39975f5-4986-43b9-8e93-ef417f1644c8"),
        Sku = "RING-004",
        Name = "Emerald Cocktail Ring",
        Description = "Vintage style emerald cocktail ring",
        CategoryId = Guid.Parse("b5c9f4e1-209c-45ab-a7ab-9cd24791ee1d"),
        UnitCost = 2200,
        Hallmark = HallmarkType.LuxuryBrands,
        WeightUnit = WeightUnit.Grams,
        Weight = 6.8f,
        UnitPrice = 3800,
        CurrentStock = 1,
        IsUniquePiece = true,
        ImageUrl = "ring004a.jpg"
    },
    new Product
    {
        Id = Guid.Parse("300ba622-ce70-4959-aa0e-f57e82a0717a"),
        Sku = "RING-002",
        Name = "Sapphire and Diamond Band",
        Description = "Blue sapphire center stone with diamond accents",
        CategoryId = Guid.Parse("b5c9f4e1-209c-45ab-a7ab-9cd24791ee1d"),
        UnitCost = 1800,
        Hallmark = HallmarkType.LuxuryBrands,
        WeightUnit = WeightUnit.Grams,
        Weight = 4.2f,
        UnitPrice = 3200,
        OfferPrice = 2850,
        CurrentStock = 3,
        IsUniquePiece = false,
        ImageUrl = "ring002a.png"
    }
};


            return products;
        }

        public static List<Category> GetCategories()
        {
            var categories = new List<Category>
{
    new Category
    {
        Id = Guid.Parse("819a8a81-0a8d-4fc2-8321-6d499e513517"),
        Name = "Bracelets",
        Description = "Charming bracelets and bangles",
        Icon = "bracelet.png"
    },
    new Category
    {
        Id = Guid.Parse("1ef1f3e6-e894-48b0-9fce-7d80d67b4d8e"),
        Name = "Necklaces",
        Description = "Elegant necklaces and pendants",
        Icon = "necklace.png"
    },
    new Category
    {
        Id = Guid.Parse("31aa9977-f86d-41ac-b943-7dbfadf3f39f"),
        Name = "Anklets",
        Description = "Delicate ankle bracelets",
        Icon = "anklet.png"
    },
    new Category
    {
        Id = Guid.Parse("b5c9f4e1-209c-45ab-a7ab-9cd24791ee1d"),
        Name = "Rings",
        Description = "Beautiful rings for every occasion",
        Icon = "ring.png"
    },
    new Category
    {
        Id = Guid.Parse("0948a137-7c69-44a0-801c-bbeed85e53d9"),
        Name = "Bridal",
        Description = "Wedding and engagement jewelry",
        Icon = "wedding.png"
    },
    new Category
    {
        Id = Guid.Parse("37343a37-6d90-4def-b8cb-bd56f225a753"),
        Name = "Cufflinks",
        Description = "Stylish cufflinks for formal wear",
        Icon = "cufflink.png"
    },
    new Category
    {
        Id = Guid.Parse("bc5ca235-21c0-45b7-ad1f-c87c134cb9b5"),
        Name = "Brooches",
        Description = "Elegant brooches and pins",
        Icon = "brooch.png"
    },
    new Category
    {
        Id = Guid.Parse("86c32353-7b9d-46ea-b124-e2469c21a679"),
        Name = "Elect",
        Description = "Electronic and accessories",
        Icon = "ar.png"
    },
    new Category
    {
        Id = Guid.Parse("7ebe85e8-6b91-40c9-a32f-e626f02f36a9"),
        Name = "Earrings",
        Description = "Stunning earrings in various styles",
        Icon = "earring.png"
    },
    new Category
    {
        Id = Guid.Parse("b3905666-d8a4-47f3-8413-e8e5c907b0e3"),
        Name = "Charms",
        Description = "Charm bracelets and individual charms",
        Icon = "charm.png"
    },
    new Category
    {
        Id = Guid.Parse("4efd3c63-b7de-4fa6-9a62-f76b86042a6e"),
        Name = "Watches",
        Description = "Luxury timepieces",
        Icon = "watch.png"
    }
};
            return categories;
        }

        public static List<Customer> GetCustomers()
        {
            var customers = new List<Customer>
{
    new Customer
    {
        Id = Guid.Parse("de53d77e-29ee-426f-aed9-045a1a040e22"),
        FullName = "Robert Taylor",
        PhoneNumber = "+1-555-0104",
        Email = "rtaylor@email.com",
        Address = "321 Elm Street, Houston, TX 77001",
        DateOfBirth = new DateTime(1982, 5, 18),
        Notes = "Corporate gifts for employees",
        LoyaltyPoints = 1500,
    },
    new Customer
    {
        Id = Guid.Parse("f52436f5-2a90-4fc8-ac74-40dc2498f984"),
        FullName = "Jessica Chen",
        PhoneNumber = "+1-555-0103",
        Email = "jchen@email.com",
        Address = "789 Pine Road, Chicago, IL 60601",
        DateOfBirth = new DateTime(1988, 11, 30),
        Notes = "Collects vintage brooches",
        LoyaltyPoints = 3200,
    },
    new Customer
    {
        Id = Guid.Parse("5e99e42a-61c3-487e-b8b8-a5665ef18c65"),
        FullName = "Emily Anderson",
        PhoneNumber = "+1-555-0101",
        Email = "emily.anderson@email.com",
        Address = "123 Oak Street, New York, NY 10001",
        DateOfBirth = new DateTime(1985, 3, 15),
        Notes = "VIP customer, prefers white gold",
        LoyaltyPoints = 2500,
    },
    new Customer
    {
        Id = Guid.Parse("5a62f26f-8467-40b6-92ff-f761ef1c1014"),
        FullName = "David Martinez",
        PhoneNumber = "+1-555-0102",
        Email = "david.m@email.com",
        Address = "456 Maple Ave, Los Angeles, CA 90001",
        DateOfBirth = new DateTime(1990, 7, 22),
        Notes = "Anniversary purchases in June",
        LoyaltyPoints = 1800,
    },
    new Customer
    {
        Id = Guid.Parse("53348e47-7a87-4c97-81ec-ff1a33048807"),
        FullName = "Sofia Rodriguez",
        PhoneNumber = "+1-555-0105",
        Email = "sofia.r@email.com",
        Address = "654 Cedar Lane, Miami, FL 33101",
        DateOfBirth = new DateTime(1995, 9, 8),
        Notes = "Bridal customer, wedding in December",
        LoyaltyPoints = 950,
    }
};

            return customers;
        }



    }
}
