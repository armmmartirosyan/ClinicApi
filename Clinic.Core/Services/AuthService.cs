using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using Clinic.Core.Models.Response;
using FluentValidation;
using FluentValidation.Results;

namespace Clinic.Core.Services;
 
public class AuthService(IAuthRepository authRepository, AbstractValidator<User> userValidator) : IAuthService
{
    public async Task<string> SignInAsync(SignInRequest request)
    {
        var user = await authRepository.GetUserByEmailAsync(request.Email);

        if (user == null || !VerifyPassword(request.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        //TODO: Improve the response.
        var token = "mock-jwt-token";

        return token;
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        //TODO: Improve the verification
        //using var sha256 = SHA256.Create();
        //var hashedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        //return hashedPassword == passwordHash;

        return false;
    }

    public async Task<long> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await authRepository.GetUserByEmailAsync(request.Email);
        if (existingUser != null)
        { 
            throw new InvalidDataException("Email is already registered.");
        }

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password,
            Phone = request.Phone,
            TypesId = request.TypesId,
            BirthDate = request.BirthDate,
        };

        ValidationResult result = userValidator.Validate(user);

        if (!result.IsValid)
        {
            throw new InvalidDataException(result.Errors[0].ErrorMessage);
        }

        user.Password = HashPassword(request.Password);

        var userId = await authRepository.AddUserAsync(user);

        Console.WriteLine(request.Specializations.ToString());
        //TODO: Add specialization logic

        return userId;
    }

    private string HashPassword(string password)
    {
        //TODO: Improve the implementation
        //using var sha256 = SHA256.Create();
        //return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));

        return password;
    }
}
