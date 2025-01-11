using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Models.Request;
using FluentValidation;

namespace Clinic.Infrastructure.Validators;

public class AddProcedureValidator : AbstractValidator<AddProcedureRequest>
{
    private readonly IProceduresRepository _procedureRepository;
    public AddProcedureValidator(IProceduresRepository procedureRepository)
    {
        _procedureRepository = procedureRepository;
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(v => v.Name)
           .NotEmpty().WithMessage("{PropertyName} is required.")
           .Length(2, 100).WithMessage("The length of {PropertyName} must be between 2 and 100.")
           .MustAsync(NotBeRepeatedName).WithMessage("The procedure by this name is already exists.");

        RuleFor(v => v.Price)
            .GreaterThan(0).WithMessage("Price must be a positive number.")
            .LessThanOrEqualTo(1000000).WithMessage("Price must not exceed 1,000,000 AMD.")
            .Must(HaveAtMostTwoDecimalPlaces).WithMessage("Price must have at most 2 decimal places.");
    }

    private async Task<bool> NotBeRepeatedName(string name, CancellationToken cancellationToken)
    {
        return await _procedureRepository.GetProcedureByNameAsync(name) == null;
    }

    private bool HaveAtMostTwoDecimalPlaces(decimal price)
    {
        return decimal.Round(price, 2) == price;
    }
}
