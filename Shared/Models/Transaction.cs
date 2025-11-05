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
        public Guid Id { get; set; }
        public TransactionType Type { get; set; }
        public float TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public SaleChannel? Channel { get; set; }
        public Guid? CustomerId { get; set; }
        public float? DiscountValue { get; set; } = 0;
        public Customer? Customer { get; set; }
        public ICollection<TransactionItem>? Items { get; set; } = new List<TransactionItem>();
    }
}
