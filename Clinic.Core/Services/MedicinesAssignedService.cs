using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Interfaces.Services;
using Clinic.Core.Models.DTO;
using Clinic.Core.Models.Request;
using FluentValidation;
using FluentValidation.Results;

namespace Clinic.Core.Services;
 
public class MedicinesAssignedService
    (
        IMedicinesAssignedRepository medicinesAssignedRepository,
        AbstractValidator<AddMedicinesAssignedRequest> addMedicinesAssignedValidator,
        AbstractValidator<UpdateMedicinesAssignedValidateDTO> updateMedicinesAssignedValidator
    ) : IMedicinesAssignedService
{
    public async Task<long> AddAsync(AddMedicinesAssignedRequest request)
    {
        ValidationResult validationResult = addMedicinesAssignedValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            throw new InvalidDataException(validationResult.Errors[0].ErrorMessage);
        }

        var medicinesAssigned = new MedicinesAssigned
        {
            Name = request.Name,
            Notes = request.Notes,
            DayCount = request.DayCount,
            Dose = request.Dose,
            Quantity = request.Quantity,    
            StartDate = request.StartDate,
            DoctorId = request.DoctorId,
            PatientId = request.PatientId,  
            VisitProcedureId = request.VisitProcedureId,    
        };

        return await medicinesAssignedRepository.AddAsync(medicinesAssigned);
    }

    public async Task<MedicinesAssigned> GetByIdAsync(long id)
    {
        var medicinesAssigned = await medicinesAssignedRepository.GetByIdAsync(id);

        if (medicinesAssigned == null)
        {
            throw new Exception("Not found assigned medicines with this ID.");
        }

        return medicinesAssigned;
    }

    public async Task<IEnumerable<MedicinesAssigned>> GetAllAsync()
    {
        return await medicinesAssignedRepository.GetAllAsync();
    }

    public async Task<bool> UpdateAsync(long id, UpdateMedicinesAssignedRequest request)
    {
        var validateDTO = new UpdateMedicinesAssignedValidateDTO()
        {
            Id = id,
            Name = request.Name,
            Dose = request.Dose,
            Notes = request.Notes,
            DayCount  = request.DayCount,
            Quantity = request.Quantity,
            StartDate = request.StartDate,
            PatientId= request.PatientId,
            VisitProcedureId = request.VisitProcedureId,
        };

        ValidationResult validationRes = updateMedicinesAssignedValidator.Validate(validateDTO);

        if (!validationRes.IsValid)
        {
            throw new InvalidDataException(validationRes.Errors[0].ErrorMessage);
        }

        var medicinesAssigned = await medicinesAssignedRepository.GetByIdAsync(id);

        if (request.Name != null)
        {
            medicinesAssigned!.Name = request.Name;
        }
        if (request.Notes != null)
        {
            medicinesAssigned!.Notes = request.Notes;    
        }
        if (request.Dose != null)
        {
            medicinesAssigned!.Dose = request.Dose;
        }
        if (request.StartDate != null)
        {
            medicinesAssigned!.StartDate = (DateOnly)request.StartDate;
        }
        if (request.Quantity != null)
        {
            medicinesAssigned!.Quantity = (int)request.Quantity;
        }
        if (request.DayCount != null)
        {
            medicinesAssigned!.DayCount = (int)request.DayCount;
        }
        if (request.PatientId != null)
        {
            medicinesAssigned!.PatientId = (long)request.PatientId;
        }
        if (request.VisitProcedureId != null)
        {
            medicinesAssigned!.VisitProcedureId = (long)request.VisitProcedureId;
        }

        return await medicinesAssignedRepository.UpdateAsync(medicinesAssigned!);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        bool success = await medicinesAssignedRepository.DeleteAsync(id);

        if (!success)
        {
            throw new InvalidDataException("Failed deleting the assigned medicines.");
        }

        return true;
    }
}
