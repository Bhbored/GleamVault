using Shared.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public TransactionType Type { get; set; }
        public float TotalAmount { get; set; }
        public string? Description { get; set; } = string.Empty;
        public float SubTotalAmount { get; set; }//added this anwaryooo
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public SaleChannel? Channel { get; set; }
        public Guid? CustomerId { get; set; }
        public float? DiscountValue { get; set; } = 0;
        public Customer? Customer { get; set; }
        public ICollection<TransactionItem>? Items { get; set; } = new List<TransactionItem>();
    }
}
