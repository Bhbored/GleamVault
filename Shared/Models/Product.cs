using Shared.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Sku { get; set; } = string.Empty;//for unique identification
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid CategoryId { get; set; }//one to many
        public Category Category { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public HallmarkType? Hallmark { get; set; } = HallmarkType.Unknown; //no3 el product
        public WeightUnit? WeightUnit { get; set; }//grams, carats, ounces, pennyweight, kilograms
        public decimal? Weight { get; set; }// eza kanet sa3at kermal haik nullable
        public decimal UnitSellingPrice { get; set; }
        public int CurrentStock { get; set; } = 0;
        public bool IsUniquePiece { get; set; } = false;

    }
}
