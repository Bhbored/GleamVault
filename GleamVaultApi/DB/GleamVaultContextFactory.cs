using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace GleamVaultApi.DB
{
    public class GleamVaultContextFactory : IDesignTimeDbContextFactory<GleamVaultContext>
    {
        public GleamVaultContext CreateDbContext(string[] args)
        {

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();


            var optionsBuilder = new DbContextOptionsBuilder<GleamVaultContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new GleamVaultContext(optionsBuilder.Options);
        }
    }
}
