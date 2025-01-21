using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Helpers;
using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using FluentValidation;
using FluentValidation.Results;

namespace Clinic.Core.Services;
 
public class AuthService
    (
        IAuthRepository authRepository, 
        AbstractValidator<RegisterRequest> userValidator,
        ITokenHelper tokenHelper
    ) : IAuthService
{
    public async Task<string> SignInAsync(SignInRequest request)
    {
        var user = await authRepository.GetUserByEmailAsync(request.Email);

        if (user == null || !VerifyPassword(request.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var token = tokenHelper.Create(user);

        return token;
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        //TODO: Improve the verification
        //using var sha256 = SHA256.Create();
        //var hashedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        //return hashedPassword == passwordHash;

        return password == passwordHash;
    }

    public async Task<long> RegisterAsync(RegisterRequest request)
    {
        ValidationResult result = userValidator.Validate(request);

        if (!result.IsValid)
        {
            throw new InvalidDataException(result.Errors[0].ErrorMessage);
        }

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            TypesId = request.TypesId,
            BirthDate = request.BirthDate,
            Password = HashPassword(request.Password),
        };

        var userId = await authRepository.AddUserAsync(user);

        if (userId == null || userId < 1)
        {
            throw new InvalidDataException("Failed registering user.");
        }

        var isDoctorTypeId = await authRepository.IsDoctorTypeId(request.TypesId);

        if (!isDoctorTypeId)
        {
            return userId;
        }

        List<DoctorsSpecialization> doctorsSpecializations = new List<DoctorsSpecialization>();

        foreach (var specializationId in request.Specializations)
        {
            doctorsSpecializations.Add(new DoctorsSpecialization()
            {
                DoctorId = userId,
                SpecializationId = specializationId
            });
        }

        await authRepository.AssignSpecializationsToUser(doctorsSpecializations);

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
