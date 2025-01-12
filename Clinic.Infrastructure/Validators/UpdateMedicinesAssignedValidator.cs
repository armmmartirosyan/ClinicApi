using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Models.DTO;
using FluentValidation;

namespace Clinic.Infrastructure.Validators;

public class UpdateMedicinesAssignedValidator : AbstractValidator<UpdateMedicinesAssignedValidateDTO>
{
    private readonly IMedicinesAssignedRepository _medicinesAssignedRepository;
    public UpdateMedicinesAssignedValidator(IMedicinesAssignedRepository medicinesAssignedRepository)
    {
        _medicinesAssignedRepository = medicinesAssignedRepository;
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MustAsync(BeAValidMedicineAssignedId).WithMessage("{PropertyName} is invalid.");

        RuleFor(v => v.Name)
           .Length(2, 100)
           .When(v => v.Name != null)
           .WithMessage("The length of {PropertyName} must be between 2 and 100.");

        RuleFor(v => v.Dose)
           .MinimumLength(1)
           .When(v => v.Dose != null)
           .WithMessage("The length of {PropertyName} must be at least 1.");

        RuleFor(v => v.Notes)
           .MaximumLength(500)
           .When(v => v.Notes != null)
           .WithMessage("The length of {PropertyName} must be less then 500.");

        RuleFor(v => v.StartDate)
            .Must(BeAValidDate)
            .When(v => v.StartDate != null)
            .WithMessage("Invalid date.")
            .Must(BeInFuture)
            .When(v => v.StartDate != null)
            .WithMessage("Date must be in the future.");

        RuleFor(v => v.Quantity)
            .GreaterThanOrEqualTo(1)
            .When(v => v.Quantity != null)
            .WithMessage("The {PropertyName} must be greater than or equal 1.");

        RuleFor(v => v.DayCount)
            .GreaterThanOrEqualTo(1)
            .When(v => v.DayCount != null)
            .WithMessage("The {PropertyName} must be greater than or equal 1.");

        RuleFor(v => v.PatientId)
            .MustAsync(BeAValidUserId)
            .When(v => v.PatientId != null)
            .WithMessage("{PropertyName} is invalid.");

        RuleFor(v => v.VisitProcedureId)
            .MustAsync(BeAValidVisitProcedureId)
            .When(v => v.VisitProcedureId != null)
            .WithMessage("{PropertyName} is invalid.");

        RuleFor(v => v)
            .MustAsync(PatientAndDoctorNotBeTheSamePerson)
            .When(v => v.PatientId != null)
            .WithMessage("DoctorId and PatientId cannot be the same.");
    }

    private bool BeAValidDate(DateOnly? date)
    {
        return date != null && date != default(DateOnly) && date.Value.Day > 0;
    }

    private bool BeInFuture(DateOnly? date)
    {
        var actualDate = DateOnly.FromDateTime(DateTime.Now);
        return date >= actualDate;
    }

    private async Task<bool> BeAValidUserId(long? userId, CancellationToken cancellationToken)
    {
        return await _medicinesAssignedRepository.IsValidUserIdAsync(userId.Value);
    }

    private async Task<bool> BeAValidMedicineAssignedId(long id, CancellationToken cancellationToken)
    {
        return await _medicinesAssignedRepository.IsValidMedicineAssignedId(id);
    }

    private async Task<bool> BeAValidVisitProcedureId(long? visitProcedureId, CancellationToken cancellationToken)
    {
        return await _medicinesAssignedRepository.IsValidVisirProcedureIdAsync(visitProcedureId.Value);
    }

    private async Task<bool> PatientAndDoctorNotBeTheSamePerson(UpdateMedicinesAssignedValidateDTO dto, CancellationToken cancellationToken)
    {
        var medicineAssigned = await _medicinesAssignedRepository.GetByIdAsync(dto.Id);

        return medicineAssigned.DoctorId != dto.PatientId;
    }
}
