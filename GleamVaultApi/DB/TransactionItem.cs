using GleamVaultApi.DAL.Contracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public int Quantity { get; set; }
        [Required]
        public float UnitPriceAtSale { get; set; }
        [Required]
        public float UnitCostAtSale { get; set; }
       

        public virtual Product Product { get; set; }
        public virtual Transaction Transaction { get; set; }
    }
}
