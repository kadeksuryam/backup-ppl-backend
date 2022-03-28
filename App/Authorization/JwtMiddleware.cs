using App.Repositories;

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
            var token = ctx.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last();
            var userId = jwtUtils.ValidateToken(token);

            // i don't think it's good idea to hit DB for every request, so ill just save the userId in ctx
            ctx.Items["userId"] = userId;
/*            if(userId != null)
            {
                ctx.Items["User"] = await _userRepository.GetById(userId.Value);
            }*/

            await _next(ctx);
        }
    }
}
