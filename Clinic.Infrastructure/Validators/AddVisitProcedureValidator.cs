using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Models.Request;
using FluentValidation;

namespace Clinic.Infrastructure.Validators;

public class AddVisitProcedureValidator : AbstractValidator<AddVisitProcedureRequest>
{
    private readonly IVisitProcedureRepository _visitProcedureRepository;
    public AddVisitProcedureValidator(IVisitProcedureRepository visitProcedureRepository)
    {
        _visitProcedureRepository = visitProcedureRepository;
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(v => v.ProcedureId)
           .NotEmpty().WithMessage("{PropertyName} is required.")
           .MustAsync(BeAValidProcedureId).WithMessage("Invalid procedure ID.");

        RuleFor(v => v.VisitId)
           .NotEmpty().WithMessage("{PropertyName} is required.")
           .MustAsync(BeAValidVisitId).WithMessage("Invalid visit ID.");

        RuleFor(v => v.Notes)
            .MaximumLength(500)
            .When(v => v.Notes != null)
            .WithMessage("Notes cannot exceed 500 characters.");
    }

    private async Task<bool> BeAValidProcedureId(long procedureId, CancellationToken cancellationToken)
    {
        return await _visitProcedureRepository.IsValidProcedureIdAsync(procedureId);
    }

    private async Task<bool> BeAValidVisitId(long visitId, CancellationToken cancellationToken)
    {
        return await _visitProcedureRepository.IsValidVisitIdAsync(visitId);
    }
}
