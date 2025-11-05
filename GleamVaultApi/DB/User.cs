using GleamVaultApi.DAL.Contracts;
using Shared.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace GleamVaultApi.DB
{
    public partial class User 
    {

        public User() 
        {
            Transaction=new HashSet<Transaction>();
        }
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }
         
        [MaxLength(150)]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        [MaxLength(500)]
        public string ApiKeyHash { get; set; }
        [Required]
        public UserRole Role { get; set; } 
        public bool IsActive { get; set; } = true;
              
        public ICollection<Transaction> Transaction { get; set; }
        
       
    }
}
