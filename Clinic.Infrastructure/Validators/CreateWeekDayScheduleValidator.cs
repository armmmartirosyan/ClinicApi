using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Models.Request;
using FluentValidation;

namespace Clinic.Infrastructure.Validators;

public class CreateWeekDayScheduleValidator : AbstractValidator<CreateWeekDayScheduleRequest>
{
    private readonly IWeekDayScheduleRepository _weekDayScheduleRepository;
    public CreateWeekDayScheduleValidator(IWeekDayScheduleRepository weekDayScheduleRepository)
    {
        _weekDayScheduleRepository = weekDayScheduleRepository;
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("The {PropertyName} is required.");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("The {PropertyName} is required.")
            .GreaterThan(x => x.StartTime).WithMessage("The {PropertyName} must be greater than Start time.");

        RuleFor(x => x.BreakStartTime)
            .NotEmpty().WithMessage("The {PropertyName} is required.")
            .GreaterThan(x => x.StartTime).WithMessage("The {PropertyName} must be greater than Start time.");

        RuleFor(x => x.BreakEndTime)
            .NotEmpty().WithMessage("The {PropertyName} is required.")
            .GreaterThan(x => x.BreakStartTime).WithMessage("The {PropertyName} must be greater than Break start time.")
            .LessThan(x => x.EndTime).WithMessage("The {PropertyName} must be less than End time.");

        RuleFor(x => x.WeekDayId)
            .NotEmpty().WithMessage("The {PropertyName} is required.")
            .MustAsync(BeAValidWeekDayIdAsync).WithMessage("Invalid Week day Id");

        RuleFor(x => x.DoctorId)
            .NotEmpty().WithMessage("The {PropertyName} is required.")
            .MustAsync(BeAValidUserIdAsync).WithMessage("Invalid Doctor Id");
    }

    private async Task<bool> BeAValidUserIdAsync(long doctorId, CancellationToken cancellationToken)
    {
        return await _weekDayScheduleRepository.IsValidDoctorIdAsync(doctorId);
    }

    private async Task<bool> BeAValidWeekDayIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _weekDayScheduleRepository.IsValidWeekDayIdAsync(id);
    }
}
