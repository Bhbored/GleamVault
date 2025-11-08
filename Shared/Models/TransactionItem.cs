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
        public string Sku { get; set; } = string.Empty;//for unique identification
        public string? Description { get; set; }
        public string? Name { get; set; }//added this anwaryooo
        public Guid CategoryId { get; set; }//one to many
        public Category Category { get; set; } = null!;
        public string ImageUrl { get; set; } = "default_product.gif";
        public HallmarkType? Hallmark { get; set; } = HallmarkType.LuxuryBrands; //no3 el product
        public WeightUnit? WeightUnit { get; set; }//grams, carats, ounces, pennyweight, kilograms
        public float? Weight { get; set; }// eza kanet sa3at kermal haik nullable
        public float OfferPrice { get; set; } = 0;
        public bool IsUniquePiece { get; set; } = false;
        public Guid ProductId { get; set; }
        public int Quantity { get; set; } = 1;
        public float UnitPrice { get; set; } = 0;
        public float UnitCost { get; set; } = 0;

    }
}
