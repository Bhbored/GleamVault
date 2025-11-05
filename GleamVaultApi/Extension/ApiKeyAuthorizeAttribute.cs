using GleamVaultApi.DB;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace GleamVaultApi.Extension
{
    public class ApiKeyAuthorizeAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.Items["User"] as User;
            if (user == null)
            {
                context.Result = new UnauthorizedObjectResult(new { error = "User not authenticated" });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }
    }
}
