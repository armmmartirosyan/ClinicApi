using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Models.Request;
using FluentValidation;

namespace Clinic.Infrastructure.Validators;

public class AddUpdateSpecializationValidator : AbstractValidator<AddUpdateSpecializationRequest>
{
    private readonly ISpecializationsRepository _specializationRepository;
    public AddUpdateSpecializationValidator(ISpecializationsRepository specializationsRepository)
    {
        _specializationRepository = specializationsRepository;
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(v => v.Name)
           .NotEmpty().WithMessage("{PropertyName} is required.")
           .Length(2, 100).WithMessage("The length of {PropertyName} must be between 2 and 100.")
           .MustAsync(NotBeRepeatedName).WithMessage("The procedure by this name is already exists.");
    }

    private async Task<bool> NotBeRepeatedName(string name, CancellationToken cancellationToken)
    {
        return await _specializationRepository.GetByNameAsync(name) == null;
    }
}
