using GleamVaultApi.DAL.Contracts;
using System.ComponentModel.DataAnnotations;

namespace GleamVaultApi.DB
{
    public partial class Category 
    {

        public Category() 
        {
            Product = new HashSet<Product>();
        }
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }  
        
        public string? Icon {  get; set; }
    
       

        public ICollection<Product> Product { get; set; } 
    }
}
