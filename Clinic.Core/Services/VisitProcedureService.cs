using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using FluentValidation;
using FluentValidation.Results;

namespace Clinic.Core.Services;
 
public class VisitProcedureService
    (
        IVisitProcedureRepository visitProcedureRepository,
        AbstractValidator<AddVisitProcedureRequest> addVisitProcedureValidator,
        AbstractValidator<UpdateVisitProcedureRequest> updateVisitProcedureValidator
    ) : IVisitProcedureService
{
    public async Task<long> AddAsync(AddVisitProcedureRequest request)
    {
        ValidationResult validationResult = addVisitProcedureValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            throw new InvalidDataException(validationResult.Errors[0].ErrorMessage);
        }

        var visitsProcedure = new VisitsProcedure
        {
            VisitId = request.VisitId,
            ProcedureId = request.ProcedureId,
            Notes = request.Notes,
        };

        return await visitProcedureRepository.AddAsync(visitsProcedure);
    }

    public async Task<VisitsProcedure> GetByIdAsync(long id)
    {
        var visitProcedure = await visitProcedureRepository.GetByIdAsync(id);

        if (visitProcedure == null)
        {
            throw new Exception("Not found visit procedure with this ID.");
        }

        return visitProcedure;
    }

    public async Task<IEnumerable<VisitsProcedure>> GetAllAsync()
    {
        return await visitProcedureRepository.GetAllAsync();
    }

    public async Task<bool> UpdateAsync(long id, UpdateVisitProcedureRequest request)
    {
        var visitProcedure = await visitProcedureRepository.GetByIdAsync(id);

        if(visitProcedure == null)
        {
            throw new InvalidDataException("Visit  procedure not found.");
        }

        ValidationResult validationRes = updateVisitProcedureValidator.Validate(request);

        if (!validationRes.IsValid)
        {
            throw new InvalidDataException(validationRes.Errors[0].ErrorMessage);
        }

        visitProcedure.Notes = request.Notes;   

        return await visitProcedureRepository.UpdateAsync(visitProcedure!);
    }

    public async Task DeleteAsync(long id)
    {
        bool success = await visitProcedureRepository.DeleteAsync(id);

        if (!success)
        {
            throw new InvalidDataException("Failed deleting the visitprocedure.");
        }
    }
}
