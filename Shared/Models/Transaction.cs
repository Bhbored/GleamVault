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
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public SaleChannel? Channel { get; set; }
        public string? PaymentMethod { get; set; }
        public ICollection<TransactionItem> Items { get; set; } = new List<TransactionItem>();
    }
}
