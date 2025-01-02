using Clinic.Core.Domain;
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

        //TODO: Improve the response.
        var token = "mock-jwt-token";

        return new SignInResponse
        {
            Token = token,
        };
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        //TODO: Improve the verification
        //using var sha256 = SHA256.Create();
        //var hashedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        //return hashedPassword == passwordHash;

        return false;
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await authRepository.GetUserByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        //TODO: Use FluentValidation
        //TODO: Use Multilingual Support

        //TODO: Check for strong password
        //TODO: Validate first name, last name
        //TODO: Validate email
        //TODO: Validate phone
        //TODO: Validate birthdate
        //TODO: Validate typeid
        //TODO: Validate specialization

        var passwordHash = HashPassword(request.Password);

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = passwordHash,
            Phone = request.Phone,
            TypesId = request.TypesId,
            BirthDate = request.BirthDate,
        };

        var userId = await authRepository.AddUserAsync(user);

        foreach (int specialization in request.Specializations)
        {
            Console.WriteLine(specialization);
            //TODO: Add specialization logic
        }

        return new RegisterResponse
        {
            UserId = userId,
            Message = "Registration successful."
        };
    }

    private string HashPassword(string password)
    {
        //TODO: Improve the implementation
        //using var sha256 = SHA256.Create();
        //return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));

        return password;
    }
}
