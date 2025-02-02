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
        IAuthHelper authHelper,
        IAuthRepository authRepository, 
        AbstractValidator<RegisterRequest> userValidator
    ) : IAuthService
{
    public async Task<string> SignInAsync(SignInRequest request)
    {
        var user = await authRepository.GetUserWithRoleByEmailAsync(request.Email);

        bool isValidPassword = authHelper.VerifyPassword(request.Password, user.Password);

        if (user == null || !isValidPassword)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var token = authHelper.GenerateToken(user);

        return token;
    }

    public async Task<string> RegisterAsync(RegisterRequest request)
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
            Password = authHelper.HashPassword(request.Password),
        };

        var userId = await authRepository.AddUserAsync(user);

        if (userId == null || userId < 1)
        {
            throw new InvalidDataException("Failed registering user.");
        }

        var isDoctorTypeId = await authRepository.IsDoctorTypeId(request.TypesId);

        string token = await SignInAsync(new SignInRequest()
        {
            Email = request.Email,
            Password = request.Password
        });

        if (!isDoctorTypeId)
        {
            return token;
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

        return token;
    }
    
    public async Task<UserType?> GetUserTypeByName(string name)
    {
        return await authRepository.GetUserTypeByName(name);
    }
}
