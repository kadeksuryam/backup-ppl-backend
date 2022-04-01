using App.Helpers;
using App.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace App.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public string Role { get; set; }
        public void OnAuthorization(AuthorizationFilterContext ctx)
        {
            // skip authorization if controller decorated with [AllowAnonymous] attribute
            var allowAnonymous = ctx.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous) return;

            // authorization
            ParsedToken? parsedToken = ctx.HttpContext.Items["userAttr"] as ParsedToken;


            if(parsedToken == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.Unauthorized, "Need authentication");
            }

            var userId = parsedToken.userId;
            var userRole = parsedToken.userRole;

            if (userId == null || userRole == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.Unauthorized, "Need authentication");
            }

            if(userRole.Equals("Customer") && Role.Equals("Admin"))
            {
                throw new HttpStatusCodeException(HttpStatusCode.Forbidden, "Admin Operation Only");
            }
        }
    }
}
