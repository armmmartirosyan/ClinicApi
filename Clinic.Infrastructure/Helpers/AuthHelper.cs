using System.Security.Claims;
using System.Text;
using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Clinic.Infrastructure.Helpers;

public class AuthHelper(IConfiguration configuration) : IAuthHelper
{
    public string GenerateToken(User user)
    {
        string secretKey = configuration["Jwt:Secret"]!;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            ]), 
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
        };

        var handler = new JsonWebTokenHandler();

        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }

    public bool VerifyPassword(string requestPassword, string password)
    {
        //TODO: Improve the verification
        //using var sha256 = SHA256.Create();
        //var hashedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        //return hashedPassword == passwordHash;

        return requestPassword == password;
    }

    public string HashPassword(string password)
    {
        //TODO: Improve the implementation
        //using var sha256 = SHA256.Create();
        //return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));

        return password;
    }
}
