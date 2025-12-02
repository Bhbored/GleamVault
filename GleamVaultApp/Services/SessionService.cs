using GleamVault.Services.Interfaces;
using Shared.Models;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace GleamVault.Services
{
    public class SessionService : ISessionService
    {
        private const string SESSION_KEY = "user_session";
        private const string API_KEY_KEY = "api_key";
        private LoginResponse? _cachedSession;

        public async Task<bool> SaveSessionAsync(LoginResponse loginResponse)
        {
            try
            {
                _cachedSession = loginResponse;
                var sessionJson = JsonSerializer.Serialize(loginResponse);

                try
                {
                    await SecureStorage.SetAsync(SESSION_KEY, sessionJson);
                    await SecureStorage.SetAsync(API_KEY_KEY, loginResponse.ApiKey);
                    return true;
                }
                catch
                {
                    Preferences.Set(SESSION_KEY, sessionJson);
                    Preferences.Set(API_KEY_KEY, loginResponse.ApiKey);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<LoginResponse?> GetSessionAsync()
        {
            try
            {
                if (_cachedSession != null)
                {
                    return _cachedSession;
                }

                string? sessionJson = null;

                try
                {
                    sessionJson = await SecureStorage.GetAsync(SESSION_KEY);
                }
                catch
                {
                    sessionJson = Preferences.Get(SESSION_KEY, null);
                }

                if (string.IsNullOrEmpty(sessionJson))
                {
                    return null;
                }

                var session = JsonSerializer.Deserialize<LoginResponse>(sessionJson);
                _cachedSession = session;
                return session;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> ClearSessionAsync()
        {
            try
            {
                _cachedSession = null;

                try
                {
                    SecureStorage.Remove(SESSION_KEY);
                    SecureStorage.Remove(API_KEY_KEY);
                }
                catch
                {
                    Preferences.Remove(SESSION_KEY);
                    Preferences.Remove(API_KEY_KEY);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsLoggedInAsync()
        {
            var session = await GetSessionAsync();
            return session != null && !string.IsNullOrEmpty(session.ApiKey);
        }

        public string? GetApiKey()
        {
            try
            {
                try
                {
                    return SecureStorage.GetAsync(API_KEY_KEY).Result;
                }
                catch
                {
                    return Preferences.Get(API_KEY_KEY, null);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
