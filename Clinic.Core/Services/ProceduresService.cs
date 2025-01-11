using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.Request;
using FluentValidation;
using FluentValidation.Results;

namespace Clinic.Core.Services;
 
public class ProceduresService
    (
        IProceduresRepository proceduresRepository, 
        AbstractValidator<AddProcedureRequest> addProcedureValidator,
        AbstractValidator<UpdateProcedureRequest> updateProcedureValidator
    ) : IProceduresService
{
    public async Task<long> AddAsync(AddProcedureRequest request)
    {
        ValidationResult validationResult = addProcedureValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            throw new InvalidDataException(validationResult.Errors[0].ErrorMessage);
        }

        var procedure = new Procedure
        {
            Name = request.Name,
            Price = request.Price,
            IsActive = request.IsActive == null ? true : request.IsActive,
        };

        return await proceduresRepository.AddProcedureAsync(procedure);
    }

    public async Task<Procedure> GetByIdAsync(long id)
    {
        var procedure = await proceduresRepository.GetProcedureByIdAsync(id);

        if (procedure == null)
        {
            throw new Exception("Not found procedure with this ID.");
        }

        return procedure;
    }

    public async Task<IEnumerable<Procedure>> GetProceduresAsync()
    {
        return await proceduresRepository.GetAllProceduresAsync();
    }

    public async Task<bool> UpdateAsync(long id, UpdateProcedureRequest request)
    {

        ValidationResult validationRes = updateProcedureValidator.Validate(request);

        if (!validationRes.IsValid)
        {
            throw new InvalidDataException(validationRes.Errors[0].ErrorMessage);
        }

        var procedure = await proceduresRepository.GetProcedureByIdAsync(id);

        if (request.Name != null)
        {
            procedure!.Name = request.Name;
        }
        if (request.Price != null)
        {
            procedure!.Price = request.Price;
        }
        if (request.IsActive != null)
        {
            procedure!.IsActive = request.IsActive;
        }

        return await proceduresRepository.UpdateProcedureAsync(procedure!);
    }

    public async Task DeleteAsync(long id)
    {
        bool success = await proceduresRepository.DeleteProcedureAsync(id);

        if (!success)
        {
            throw new InvalidDataException("Failed deleting the procedure.");
        }
    }
}
