using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Models.Request;
using FluentValidation;

namespace Clinic.Infrastructure.Validators;

public class UpdateProcedureValidator : AbstractValidator<UpdateProcedureRequest>
{
    private readonly IProceduresRepository _procedureRepository;
    public UpdateProcedureValidator(IProceduresRepository procedureRepository)
    {
        _procedureRepository = procedureRepository;
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(v => v.Name)
           .NotEmpty()
           .When(v => v.Name != null)
           .WithMessage("{PropertyName} is required.")
           .Length(2, 100)
           .When(v => v.Name != null)
           .WithMessage("The length of {PropertyName} must be between 2 and 100.")
           .MustAsync(NotBeRepeatedName)
           .When(v => v.Name != null)
           .WithMessage("The procedure by this name is already exists.");

        RuleFor(v => v.Price)
            .GreaterThan(0)
            .When(v => v.Price != null)
            .WithMessage("Price must be a positive number.")
            .LessThanOrEqualTo(1000000)
            .When(v => v.Price != null)
            .WithMessage("Price must not exceed 1,000,000 AMD.")
            .Must(HaveAtMostTwoDecimalPlaces)
            .When(v => v.Price != null)
            .WithMessage("Price must have at most 2 decimal places.");
    }

    private async Task<bool> NotBeRepeatedName(string name, CancellationToken cancellationToken)
    {
        return await _procedureRepository.GetProcedureByNameAsync(name) == null;
    }

    private bool HaveAtMostTwoDecimalPlaces(decimal? price)
    {
        if (price == null)
        {
            return false;
        }

        return decimal.Round(price.Value, 2) == price;
    }
}
