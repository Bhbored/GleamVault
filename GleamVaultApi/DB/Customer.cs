using GleamVaultApi.DAL.Contracts;

namespace GleamVaultApi.DB
{
    public partial class Customer 
    {
        public Customer() 
        {
         Transaction=new HashSet<Transaction>();
        }
       
        public Guid Id { get; set; }
        public string FullName { get; set; } 
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Notes { get; set; }
        public int LoyaltyPoints { get; set; } = 0;
        public string? ImageUrl { get; set; }
       
        public ICollection<Transaction> Transaction { get; set; } 
    }
}
