using PropertyChanged;
using Shared.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    [AddINotifyPropertyChangedInterface]

    public class TransactionItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid TransactionId { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Name { get; set; }
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; } = null!;
        public string ImageUrl { get; set; } = "default_product.gif";
        public HallmarkType? Hallmark { get; set; } = HallmarkType.LuxuryBrands; 
        public WeightUnit? WeightUnit { get; set; }
        public float? Weight { get; set; }
        public float OfferPrice { get; set; } = 0;
        public bool IsUniquePiece { get; set; } = false;
        public Guid ProductId { get; set; }
        public int Quantity { get; set; } = 1;
        public float UnitPrice { get; set; } = 0;
        public float UnitCost { get; set; } = 0;
        public DateTime CreatedDate { get; set; }


    }
}
