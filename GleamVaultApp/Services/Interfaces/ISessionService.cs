using Shared.Models;
using System;
using System.Threading.Tasks;

namespace GleamVault.Services.Interfaces
{
    public interface ISessionService
    {
        Task<bool> SaveSessionAsync(LoginResponse loginResponse);
        Task<LoginResponse?> GetSessionAsync();
        Task<bool> ClearSessionAsync();
        Task<bool> IsLoggedInAsync();
        string? GetApiKey();
    }
}
