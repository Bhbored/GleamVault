using GleamVaultApi.DB;
using System.Security.Claims;

namespace GleamVaultApi.Extension
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string API_KEY_HEADER = "X-API-Key";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, GleamVaultContext dbContext)
        {
           
            if (IsExcludedPath(context.Request.Path))
            {
                await _next(context);
                return;
            }

           
            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var apiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "API Key is missing" });
                return;
            }

          
            var user = await dbContext.ValidateApiKeyAsync(apiKey!);

            if (user == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "Invalid or inactive API Key" });
                return;
            }

           
            context.Items["User"] = user;
            context.Items["UserId"] = user.Id;
            context.Items["UserRole"] = user.Role;

           
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var identity = new ClaimsIdentity(claims, "ApiKey");
            context.User = new ClaimsPrincipal(identity);

           
            await _next(context);
        }

        private bool IsExcludedPath(PathString path)
        {
           
            var excludedPaths = new[]
            {
                "/api/auth/login",
                "/api/auth/register",
                "/health",
                "/swagger"
            };

            return excludedPaths.Any(p => path.StartsWithSegments(p, StringComparison.OrdinalIgnoreCase));
        }
    }

    
    public static class ApiKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyValidation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}

