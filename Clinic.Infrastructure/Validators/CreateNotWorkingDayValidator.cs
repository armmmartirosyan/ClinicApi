using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Models.Request;
using FluentValidation;

namespace Clinic.Infrastructure.Validators;

public class CreateNotWorkingDayValidator : AbstractValidator<CreateNotWorkingDayRequest>
{
    private readonly INotWorkingDaysRepository _notWorkingDaysRepository;
    public CreateNotWorkingDayValidator(INotWorkingDaysRepository notWorkingDaysRepository)
    {
        _notWorkingDaysRepository = notWorkingDaysRepository;
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(x => x.NotWorkDate)
            .NotEmpty().WithMessage("The {PropertyName} is required.");
    }
}
