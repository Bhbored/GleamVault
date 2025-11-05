using GleamVaultApi.DAL.Contracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared.Models.Enums;

namespace GleamVaultApi.DB
{
    public partial class Product 
    {
        public Product() 
        {
         TransactionItem=new HashSet<TransactionItem>();
        }
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Sku { get; set; } = string.Empty;
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string? Description { get; set; }
            
        public Guid CategoryId { get; set; }
        [Required]
        public float UnitCost { get; set; }

        public HallmarkType? Hallmark { get; set; }

        public WeightUnit? WeightUnit { get; set; }

        
        public float Weight { get; set; }

        [Required]
        public float UnitPrice { get; set; }

        [Required]
        public int CurrentStock { get; set; } = 0;

        public bool IsUniquePiece { get; set; } = false;

     

        public virtual Category Category { get; set; }
        public virtual ICollection<TransactionItem> TransactionItem { get; set; }
    }
}
