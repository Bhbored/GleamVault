using GleamVaultApi.DAL.Contracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared.Models.Enums;

namespace GleamVaultApi.DB
{
    public partial class TransactionItem 
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid TransactionId { get; set; }
        [Required]
        public Guid ProductId { get; set; }

        public string? Sku { get; set; }

        public string? Description { get; set; }
        public string? Name { get; set; }

        public Guid? CategoryId { get; set; }
        public string ImageUrl { get; set; } = "default_product.gif";

        public HallmarkType? Hallmark { get; set; } = HallmarkType.LuxuryBrands; 
        public WeightUnit? WeightUnit { get; set; }
        public float? Weight { get; set; }
        public float? OfferPrice { get; set; }
        public bool IsUniquePiece { get; set; } = false;
        [Required]
        public int Quantity { get; set; }
        [Required]
        public float UnitPriceAtSale { get; set; }
        [Required]
        public float UnitCostAtSale { get; set; }
       

        public virtual Product Product { get; set; }
        public virtual Transaction Transaction { get; set; }
        public virtual Category Category { get; set; }
    }
}
