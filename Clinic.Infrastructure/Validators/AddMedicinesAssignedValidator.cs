using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Models.Request;
using FluentValidation;

namespace Clinic.Infrastructure.Validators;

public class AddMedicinesAssignedValidator : AbstractValidator<AddMedicinesAssignedRequest>
{
    private readonly IMedicinesAssignedRepository _medicinesAssignedRepository;
    public AddMedicinesAssignedValidator(IMedicinesAssignedRepository medicinesAssignedRepository)
    {
        _medicinesAssignedRepository = medicinesAssignedRepository;
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(v => v.Name)
           .NotEmpty().WithMessage("{PropertyName} is required.")
           .Length(2, 100).WithMessage("The length of {PropertyName} must be between 2 and 100.");

        RuleFor(v => v.Dose)
           .NotEmpty().WithMessage("{PropertyName} is required.")
           .MinimumLength(1).WithMessage("The length of {PropertyName} must be at least 1.");

        RuleFor(v => v.Notes)
           .MaximumLength(500)
           .When(v => v.Notes != null)
           .WithMessage("The length of {PropertyName} must be less then 500.");

        RuleFor(v => v.StartDate)
            .NotEmpty().WithMessage("The {PropertyName} is required.")
            .Must(BeAValidDate).WithMessage("Invalid date.")
            .Must(BeInFuture).WithMessage("Date must be in the future.");

        RuleFor(v => v.Quantity)
            .NotEmpty().WithMessage("The {PropertyName} is required.")
            .GreaterThanOrEqualTo(1).WithMessage("The {PropertyName} must be greater than or equal 1.");

        RuleFor(v => v.DayCount)
            .NotEmpty().WithMessage("The {PropertyName} is required.")
            .GreaterThanOrEqualTo(1).WithMessage("The {PropertyName} must be greater than or equal 1.");

        RuleFor(v => v.PatientId)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MustAsync(BeAValidUserId).WithMessage("{PropertyName} is invalid.");

        RuleFor(v => v.DoctorId)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MustAsync(BeAValidDoctorId).WithMessage("{PropertyName} is invalid.");

        RuleFor(v => v.VisitProcedureId)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MustAsync(BeAValidVisitProcedureId).WithMessage("{PropertyName} is invalid.");

        RuleFor(v => v)
            .Must(v => v.DoctorId != v.PatientId).WithMessage("DoctorId and PatientId cannot be the same.");
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

    private async Task<bool> BeAValidUserId(long userId, CancellationToken cancellationToken)
    {
        return await _medicinesAssignedRepository.IsValidUserIdAsync(userId);
    }

    private async Task<bool> BeAValidDoctorId(long doctorId, CancellationToken cancellationToken)
    {
        return await _medicinesAssignedRepository.IsValidDoctorId(doctorId);
    }

    private async Task<bool> BeAValidVisitProcedureId(long visitProcedureId, CancellationToken cancellationToken)
    {
        return await _medicinesAssignedRepository.IsValidVisirProcedureIdAsync(visitProcedureId);
    }
}
