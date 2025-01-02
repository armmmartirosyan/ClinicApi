using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using Clinic.Core.Models.Response;

namespace Clinic.Core.Services;

public class AuthService(IAuthRepository authRepository) : IAuthService
{
    public async Task<SignInResponse> SignInAsync(SignInRequest request)
    {
        var user = await authRepository.GetUserByEmailAsync(request.Email);

        if (user == null || !VerifyPassword(request.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        // Generate token and refresh token (mocked for simplicity)
        var token = "mock-jwt-token";

        return new SignInResponse
        {
            Token = token,
        };
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        //using var sha256 = SHA256.Create();
        //var hashedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        //return hashedPassword == passwordHash;

        return false;
    }
}
