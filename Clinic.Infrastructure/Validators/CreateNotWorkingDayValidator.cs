using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Models.Request;
using FluentValidation;

namespace Clinic.Infrastructure.Validators;

public class CreateNotWorkingDayValidator : AbstractValidator<CreateNotWorkingDayRequestValidator>
{
    private readonly INotWorkingDaysRepository _notWorkingDaysRepository;
    public CreateNotWorkingDayValidator(INotWorkingDaysRepository notWorkingDaysRepository)
    {
        _notWorkingDaysRepository = notWorkingDaysRepository;
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(x => x.NotWorkDate)
            .NotEmpty().WithMessage("The {PropertyName} is required.")
            .Must(BeAValidDate).WithMessage("Invalid date.")
            .Must(BeInFuture).WithMessage("Date must be in the future.");

        RuleFor(x => x.DoctorId)
            .NotEmpty().WithMessage("The {PropertyName} is required.")
            .MustAsync(BeAValidDoctorId).WithMessage("Invalid doctor ID.");

        RuleFor(x => x)
            .MustAsync(DateNotBeRegisteredAlready).WithMessage("Date already registered.");
    }

    private bool BeAValidDate(DateOnly date)
    {
        return date != default(DateOnly) && date.Day > 0;
    }

    private bool BeInFuture(DateOnly date)
    {
        var nextCalendarDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        return date >= nextCalendarDate;
    }

    private async Task<bool> BeAValidDoctorId(long doctorId, CancellationToken cancellationToken)
    {
        return await _notWorkingDaysRepository.IsValidDoctorIdAsync(doctorId);
    }

    private async Task<bool> DateNotBeRegisteredAlready(CreateNotWorkingDayRequestValidator request, CancellationToken cancellationToken)
    {
        return await _notWorkingDaysRepository.DateNotRegisteredAlready(request.DoctorId, request.NotWorkDate);
    }
}
