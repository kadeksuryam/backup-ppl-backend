using App.Helpers;
using App.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace App.Authorization
{
    public class JwtUtils : IJwtUtils
    {
        string jwtSecret;

        public JwtUtils() {
            string? envJwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
            jwtSecret = envJwtSecret == null ? "" : envJwtSecret;
        }


        public string GenerateToken(User user)
        {
            // generate token that is valid for 1 day
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()), new Claim("role", user.Role.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ParsedToken? ValidateToken(string? token)
        {
            if (token == null) return null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
       
                uint? userId = null;
                uint tmpUserId;
                if(uint.TryParse(jwtToken.Claims.First(x => x.Type == "id").Value, out tmpUserId))
                {
                    userId = tmpUserId;
                }
                var userRole = jwtToken.Claims.Last(x => x.Type == "role").Value;

                return new ParsedToken(userId, userRole);

            } catch (Exception ex)
            {
                return null;
            }
        }
    }
}
