using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Models.Request;
using FluentValidation;

namespace Clinic.Infrastructure.Validators;

public class UpdateVisitValidator : AbstractValidator<UpdateVisitRequest>
{
    private readonly IVisitRepository _visitRepository; 
    public UpdateVisitValidator(IVisitRepository visitRepository)
    {
        _visitRepository = visitRepository;
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(v => v.Notes)
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");

        RuleFor(v => v.StartScheduledDate)
            .Must(BeAValidDate).WithMessage("{PropertyName} must be a valid date.");

        RuleFor(v => v.EndScheduledDate)
            .Must(BeAValidDate).WithMessage("{PropertyName} must be a valid date.");

        RuleFor(v => v.StartActualDate)
            .Must(BeAValidDate).WithMessage("{PropertyName} must be a valid date.");

        RuleFor(v => v.EndActualDate)
            .Must(BeAValidDate).WithMessage("{PropertyName} must be a valid date.");

        RuleFor(v => v.StatusId)
            .MustAsync(BeAValidStatusIdAsync).WithMessage("{PropertyName} must be a valid visit status id.");

        RuleFor(v => v)
            .Must(v => v.StartScheduledDate <= v.EndScheduledDate).WithMessage("End schedule date can't be earlier than start schedule date.")
            .Must(v => v.StartActualDate <= v.EndActualDate).WithMessage("End actual date can't be earlier than start actual date.");
    }

    private bool BeAValidDate(DateTime? date)
    {
        return date != null && date != default(DateTime);
    }

    private async Task<bool> BeAValidStatusIdAsync(int?  statusId, CancellationToken cancellationToken)
    {
        return statusId != null && await _visitRepository.IsValidStatusIdAsync(statusId);
    }
}
