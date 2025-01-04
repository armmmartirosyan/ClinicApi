using System.Text.RegularExpressions;
using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using FluentValidation;

namespace Clinic.Infrastructure.Validators;

public class UserValidator : AbstractValidator<User>
{
    private readonly IAuthRepository _authRepository;
    public UserValidator(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(u => u.FirstName)
            .NotEmpty().WithMessage("{PropertyName} is empty")
            .Length(2, 50).WithMessage("Length of {PropertyName} invalid")
            .Must(BeAValidName).WithMessage("{PropertyName} contains invalid characters");

        RuleFor(u => u.LastName)
            .NotEmpty().WithMessage("{PropertyName} is empty")
            .Length(2, 50).WithMessage("Length of {PropertyName} invalid")
            .Must(BeAValidName).WithMessage("{PropertyName} contains invalid characters");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Must(ContainUppercase).WithMessage("Password must contain at least one uppercase letter.")
            .Must(ContainLowercase).WithMessage("Password must contain at least one lowercase letter.")
            .Must(ContainDigit).WithMessage("Password must contain at least one digit.")
            .Must(ContainSpecialCharacter).WithMessage("Password must contain at least one special character.");

        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is required.")
            .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
            .WithMessage("Email is invalid.");

        RuleFor(u => u.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+\d{11,15}$").WithMessage("Invalid phone number.");

        RuleFor(u => u.BirthDate)
           .NotEmpty().WithMessage("Birthdate is required.")
           .Must(BeAValidDate).WithMessage("Birthdate must be a valid date.")
           .Must(NotBeInTheFuture).WithMessage("Birthdate cannot be in the future.");

        RuleFor(u => u.TypesId)
           .NotEmpty().WithMessage("UserTypeId is required.")
           .MustAsync(ValidateUserTypeId).WithMessage("UserTypeId is invalid.");

        //RuleFor(x => x.SpecializationIds)
        //    .NotEmpty().WithMessage("Specializations are required.")
        //    .MustAsync(ValidateSpecializationIds).WithMessage("One or more specialization IDs are invalid.");
    }

    private bool BeAValidName(string name)
    {
        name = name.Replace(" ", "");
        name = name.Replace("-", "");

        return name.All(Char.IsLetter);
    }

    private bool ContainUppercase(string password)
    {
        return password.Any(char.IsUpper);
    }

    private bool ContainLowercase(string password)
    {
        return password.Any(char.IsLower);
    }

    private bool ContainDigit(string password)
    {
        return password.Any(char.IsDigit);
    }

    private bool ContainSpecialCharacter(string password)
    {
        var specialCharacterRegex = new Regex(@"[!@#$%^&*(),.?\""{}|<>\\[\]~`_\-+=/]");
        return specialCharacterRegex.IsMatch(password);
    }
    private bool BeAValidDate(DateOnly date)
    {
        return date != default(DateOnly);
    }

    private bool NotBeInTheFuture(DateOnly date)
    {
        return date <= DateOnly.FromDateTime(DateTime.Today);
    }

    private async Task<bool> ValidateUserTypeId(int userTypeId, CancellationToken cancellationToken)
    {
        return await _authRepository.IsValidUserTypeIdAsync(userTypeId);
    }

    //private async Task<bool> ValidateSpecializationIds(List<int> specializationIds, CancellationToken cancellationToken)
    //{
    //    if (specializationIds == null || !specializationIds.Any())
    //        return false;

    //    foreach (var specializationId in specializationIds)
    //    {
    //        if (!await _specializationService.IsValidSpecializationIdAsync(specializationId))
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}
}
