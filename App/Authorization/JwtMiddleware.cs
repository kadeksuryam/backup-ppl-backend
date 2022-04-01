using App.Helpers;
using App.Repositories;
using System.Net;

namespace App.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext ctx, IUserRepository _userRepository, IJwtUtils jwtUtils)
        {
            string? token = ctx.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last();
            ParsedToken? parsedToken = jwtUtils.ValidateToken(token);

            ctx.Items["userAttr"] = parsedToken;

            await _next(ctx);
        }
    }
}
