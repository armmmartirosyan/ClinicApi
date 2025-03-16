using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using FluentValidation;
using FluentValidation.Results;
using Clinic.Core.Models.DTO;

namespace Clinic.Core.Services;

public class VisitService
    (
        IVisitRepository visitRepository, 
        AbstractValidator<AddVisitRequest> addVisitValidator, 
        AbstractValidator<UpdateVisitRequest> updateVisitValidator
    ) : IVisitService
{
    public async Task<long> AddVisitAsync(AddVisitRequest request)
    {
        ValidationResult result = addVisitValidator.Validate(request);

        if (!result.IsValid)
        {
            throw new InvalidDataException(result.Errors[0].ErrorMessage);
        }

        VisitStatus pendingStatus = await visitRepository.GetVisitStatusAsync("Pending");

        var visit = new Visit
        {
            PatientId = request.PatientId,
            DoctorId = request.DoctorId,
            StartScheduledDate = request.StartScheduledDate,
            EndScheduledDate = request.EndScheduledDate,
            Notes = request.Notes,
            StatusId = pendingStatus.Id,
        };

        return await visitRepository.AddVisitAsync(visit);
    }

    public async Task<List<Visit>> GetAllVisitsAsync(DecodedTokenDTO decodedToken)
    {
        return await visitRepository.GetAllVisitsAsync(decodedToken);
    }

    public async Task<Visit?> GetVisitByIdAsync(long id)
    {
        return await visitRepository.GetVisitByIdAsync(id);
    }

    public async Task<bool> UpdateVisitAsync(long id, UpdateVisitRequest request)
    {
        var visit = await visitRepository.GetVisitByIdAsync(id);

        if (visit == null)
        {
            throw new InvalidDataException("There is no visit of this ID.");
        }

        ValidationResult result = updateVisitValidator.Validate(request);

        if (!result.IsValid)
        {
            throw new InvalidDataException(result.Errors[0].ErrorMessage);
        }

        if (request.StartScheduledDate.HasValue)
            visit.StartScheduledDate = request.StartScheduledDate.Value;

        if (request.EndScheduledDate.HasValue)
            visit.EndScheduledDate = request.EndScheduledDate.Value;

        if (request.StartActualDate.HasValue)
            visit.StartActualDate = request.StartActualDate.Value;

        if (request.EndActualDate.HasValue)
            visit.EndScheduledDate = request.EndActualDate.Value;

        if (request.StatusId.HasValue)
            visit.StatusId = request.StatusId.Value;

        if (!string.IsNullOrEmpty(request.Notes))
            visit.Notes = request.Notes;

        return await visitRepository.UpdateVisitAsync(visit);
    }

    public async Task<bool> DeleteVisitAsync(long id)
    {
        return await visitRepository.DeleteVisitAsync(id);
    }
}
