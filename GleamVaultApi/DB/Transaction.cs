using GleamVaultApi.DAL.Contracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared.Models.Enums;

namespace GleamVaultApi.DB
{
    public partial class Transaction 
    {
        public Transaction() 
        {
            TransactionItem = new HashSet<TransactionItem>();
        }
        [Key]
        public Guid Id {get;set;}
        [Required]
        public TransactionType Type { get; set; }

        [Required]
        public float TotalAmount { get; set; }
        public float? DiscountValue { get; set; } = 0;

        
        public SaleChannel? Channel { get; set; }
        [Required]
        public Guid CreatedByUserId { get; set; }
        public Guid? CustomerID { get; set; }
        

        public virtual User CreatedByUser { get; set; }
        public virtual Customer? Customer { get; set; }
        public ICollection<TransactionItem> TransactionItem { get; set; }
    }
}
