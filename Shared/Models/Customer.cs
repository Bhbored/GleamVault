using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class Customer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? ImageUrl { get; set; } = "user.png";//this is readonly
        public DateTime? DateOfBirth { get; set; }
        public string? Notes { get; set; } 
        public int LoyaltyPoints { get; set; } = 0;
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
