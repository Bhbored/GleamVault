using GleamVaultApi.DB;
using Microsoft.EntityFrameworkCore;

namespace GleamVaultApi.Extension
{
    public static class UserExtension
    {
        public static async Task<User?> ValidateApiKeyAsync(this GleamVaultContext context, string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                return null;
                 
          
            var user = await context.User
                .FirstOrDefaultAsync(u => u.ApiKeyHash == apiKey && u.IsActive);

            return user;
        }

        public static async Task<Guid?> GetUserIdFromApiKeyAsync(this GleamVaultContext context, string apiKey)
        {
            var user = await context.ValidateApiKeyAsync(apiKey);
            return user?.Id;
        }
    }
}
