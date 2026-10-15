using DbDemoProject.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DbDemoProject.Helper
{
    public class JWTHelper
    {
            private readonly IConfiguration _configuration;

            public JWTHelper(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public string GenerateToken(Users user)
            {
                var claims = new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.UserUUID.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role!=null? user.Role.RoleName: null)
        };

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _configuration["Jwt:Key"]!
                    )
                );

                var credentials = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256
                );

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler()
                    .WriteToken(token);
            }
    }
}
