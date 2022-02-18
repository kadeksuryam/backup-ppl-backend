using App.Helpers;
using App.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace App.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext ctx)
        {
            // skip authorization if controller decorated with [AllowAnonymous] attribute
            var allowAnonymous = ctx.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous) return;

            // authorization
            var user = (User)ctx.HttpContext.Items["User"];
            if(user == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.Unauthorized, "Unauthorized");
            }
        }
    }
}
