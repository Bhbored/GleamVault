using GleamVaultApi.DB;
using Microsoft.EntityFrameworkCore;

namespace GleamVaultApi.DAL.Services
{
    public class DatabaseService
    {
        public static GleamVaultApi.DB.GleamVaultContext GetDB()
        {
            DbContextOptionsBuilder<GleamVaultContext> optionsBuilder = new DbContextOptionsBuilder<GleamVaultContext>();
          
            optionsBuilder.UseSqlServer("Server=HASHEM_ENGINEER;Database=GleamVault;User Id=sa;Password=;TrustServerCertificate=true;");

            var db = new GleamVaultApi.DB.GleamVaultContext(optionsBuilder.Options);

            return db;
        }
    }
}
