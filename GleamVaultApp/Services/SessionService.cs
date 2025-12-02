using GleamVault.Services.Interfaces;
using Shared.Models;
using System.Text.Json;

public class SessionService : ISessionService
{
    private const string SESSION_KEY = "user_session";
    private const string API_KEY_KEY = "api_key";
    private LoginResponse? _cachedSession;
    private string? _cachedApiKey;  

    public async Task<bool> SaveSessionAsync(LoginResponse loginResponse)
    {
        try
        {
            _cachedSession = loginResponse;
            _cachedApiKey = loginResponse.ApiKey;  

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
            _cachedApiKey = session?.ApiKey; 
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
            _cachedApiKey = null;  

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
        
        if (!string.IsNullOrEmpty(_cachedApiKey))
        {
            return _cachedApiKey;
        }

        
        try
        {
            _cachedApiKey = Preferences.Get(API_KEY_KEY, null);
            return _cachedApiKey;
        }
        catch
        {
            return null;
        }
    }
}