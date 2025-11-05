using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class TransactionItem
    {
      
        public Guid Id { get; set; }

     
        public Guid TransactionId { get; set; }
      
        public Guid ProductId { get; set; }
        
        public int Quantity { get; set; }
     
        public float UnitPriceAtSale { get; set; }
      
        public float UnitCostAtSale { get; set; }


        public virtual Product Product { get; set; }
        public virtual Transaction Transaction { get; set; }
    }
}
