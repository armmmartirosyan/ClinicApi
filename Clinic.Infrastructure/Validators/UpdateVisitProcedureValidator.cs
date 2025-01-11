using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Models.Request;
using FluentValidation;

namespace Clinic.Infrastructure.Validators;

public class UpdateVisitProcedureValidator : AbstractValidator<UpdateVisitProcedureRequest>
{
    private readonly IVisitProcedureRepository _visitProcedureRepository;
    public UpdateVisitProcedureValidator(IVisitProcedureRepository visitProcedureRepository)
    {
        _visitProcedureRepository = visitProcedureRepository;
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(v => v.Notes)
            .MaximumLength(500)
            .WithMessage("Notes cannot exceed 500 characters.");
    }
}
