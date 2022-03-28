using App.Models;

namespace App.Authorization
{
    public interface IJwtUtils
    {
        public string GenerateToken(User user);
        public ParsedToken? ValidateToken(string? token);
    }
}
