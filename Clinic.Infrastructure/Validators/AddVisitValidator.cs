using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Models.Request;
using FluentValidation;

namespace Clinic.Infrastructure.Validators;

public class AddVisitValidator : AbstractValidator<AddVisitRequest>
{
    private readonly IVisitRepository _visitRepository;
    public AddVisitValidator(IVisitRepository visitRepository)
    {
        _visitRepository = visitRepository;
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(v => v.DoctorId)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MustAsync(BeAValidDoctorId).WithMessage("{PropertyName} is invalid.");

        RuleFor(v => v.Notes)
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
        //TODO: Improve schedule date validation
        //2025-01-06T15:30:50
        RuleFor(v => v.StartScheduledDate)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Must(BeAValidDate).WithMessage("{PropertyName} must be a valid date.");

        RuleFor(v => v.EndScheduledDate)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Must(BeAValidDate).WithMessage("{PropertyName} must be a valid date.");

        RuleFor(v => v)
            .Must(v => v.StartScheduledDate <= v.EndScheduledDate).WithMessage("End schedule date can't be earlier than start schedule date.");
    }

    private async Task<bool> BeAValidUserId(long userId, CancellationToken cancellationToken)
    {
        return await _visitRepository.IsValidUserIdAsync(userId);
    }

    private async Task<bool> BeAValidDoctorId(long doctorId, CancellationToken cancellationToken)
    {
        return await _visitRepository.IsValidDoctorId(doctorId);
    }

    private bool BeAValidDate(DateTime date)
    {
        return date != default(DateTime);
    }
}
