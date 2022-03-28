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
            var userId = ctx.HttpContext.Items["userId"];
            if(userId == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.Unauthorized, "Unauthorized");
            }
        }
    }
}
