using Clinic.Core.Models.Request;
using FluentValidation;

namespace Clinic.Infrastructure.Validators;

public class UpdateWeekDayScheduleValidator : AbstractValidator<UpdateWeekDayScheduleRequest>
{
    public UpdateWeekDayScheduleValidator()
    {
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
    }
}
