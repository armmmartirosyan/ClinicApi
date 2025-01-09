using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Models.DTO;
using FluentValidation;

namespace Clinic.Infrastructure.Validators;

public class UpdateNotWorkingDayValidator : AbstractValidator<UpdateNotWorkingDateValidateDTO>
{
    private readonly INotWorkingDaysRepository _notWorkingDaysRepository;
    public UpdateNotWorkingDayValidator(INotWorkingDaysRepository notWorkingDaysRepository)
    {
        _notWorkingDaysRepository = notWorkingDaysRepository;
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(x => x)
            .MustAsync(DateNotBeRegisteredAlready).WithMessage("Date already registered or invalid ID.");

        RuleFor(x => x.NotWorkDate)
            .NotEmpty().WithMessage("The {PropertyName} is required.")
            .Must(BeAValidDate).WithMessage("Invalid date.")
            .Must(BeTodayOrInFuture).WithMessage("Date must be today or in the future.");
    }

    private bool BeAValidDate(DateOnly date)
    {
        return date != default(DateOnly);
    }

    private bool BeTodayOrInFuture(DateOnly date)
    {
        var nextCalendarDate = DateOnly.FromDateTime(DateTime.Now);
        return date > nextCalendarDate;
    }

    private async Task<bool> DateNotBeRegisteredAlready(UpdateNotWorkingDateValidateDTO dto, CancellationToken cancellationToken)
    {
        return await _notWorkingDaysRepository.DateNotRegisteredAlreadyUpdate(dto);
    }
}
