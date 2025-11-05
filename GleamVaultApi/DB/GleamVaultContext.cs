using Microsoft.EntityFrameworkCore;

namespace GleamVaultApi.DB
{
    public partial class GleamVaultContext : DbContext
    {
        public GleamVaultContext()
        {
        }

        public GleamVaultContext(DbContextOptions<GleamVaultContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<TransactionItem> TransactionItem { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            // Categories
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedBy)                   
                    .HasMaxLength(100);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100);
            });

            // Products
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Sku)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Description)
                    .HasMaxLength(1000);

                entity.Property(e => e.Hallmark)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.WeightUnit)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedBy)                  
                    .HasMaxLength(100);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

               
                entity.HasIndex(e => e.Sku)
                    .IsUnique();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.PasswordHash)
                    .IsRequired();

                entity.Property(e => e.ApiKeyHash)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Role)
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .IsRequired();

                entity.HasIndex(e => e.Username)
                    .IsUnique();

                entity.HasIndex(e => e.Email)
                    .IsUnique();
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Type)
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Channel)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.TotalAmount)
                    .IsRequired(); 

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100);
                entity.Property(e => e.DiscountValue)
                   .HasColumnType("real")
                   .HasDefaultValue(0f) 
                   .IsRequired(false);


                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Customer)
                       .WithMany(p => p.Transaction)
                       .HasForeignKey(d => d.CustomerID)
                       .OnDelete(DeleteBehavior.Restrict)
                       .IsRequired(false); 

             
            });

            modelBuilder.Entity<TransactionItem>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();


                entity.Property(e => e.UnitPriceAtSale)                  
                    .IsRequired();

                entity.Property(e => e.UnitCostAtSale)                   
                    .IsRequired();

               
                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.TransactionItem)
                    .HasForeignKey(d => d.TransactionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TransactionItem)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            //Customer
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20);

                entity.Property(e => e.Email)
                    .HasMaxLength(150);

                entity.Property(e => e.Address)
                    .HasMaxLength(500);

                entity.Property(e => e.Notes)
                    .HasMaxLength(1000);

                entity.Property(e => e.CreatedBy)                    
                    .HasMaxLength(100);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100);

            
                entity.HasIndex(e => e.Email)
                    .IsUnique()
                    .HasFilter("[Email] IS NOT NULL");

                entity.HasIndex(e => e.PhoneNumber)
                    .IsUnique()
                    .HasFilter("[PhoneNumber] IS NOT NULL");

               
                entity.HasMany(d => d.Transaction)
                    .WithOne(p => p.Customer)
                    .HasForeignKey(d => d.CustomerID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}