using System.Security.Claims;
using System.Text;
using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Helpers;
using Clinic.Core.Models.DTO;
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
                new Claim(ClaimTypes.Role, user.Types.Name)
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

    public static DecodedTokenDTO DecodeToken(string token)
    {
        string? splitToken = token.ToString().Split(' ').Last();

        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(splitToken);
        string? stringUserId = jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
        string? email = jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value;
        string? role = jwtToken.Claims.First(c => c.Type == ClaimTypes.Role).Value;

        long.TryParse(stringUserId, out long userId);

        return new DecodedTokenDTO
        {
            UserId = userId,
            Email = email,
            Role = role
        };
    }

    public bool VerifyPassword(string requestPassword, string password)
    {
        return BCrypt.Net.BCrypt.Verify(requestPassword, password);
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}
