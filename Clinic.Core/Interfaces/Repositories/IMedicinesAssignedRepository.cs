using Clinic.Core.Domain;
using Clinic.Core.Models.DTO;

namespace Clinic.Core.Interfaces.Repositories;

public interface IMedicinesAssignedRepository
{
    Task<long> AddAsync(MedicinesAssigned medicinesAssigned);
    Task<InfiniteScrollDTO<MedicinesAssigned>> GetAllAsync(int page, int pageSize, DecodedTokenDTO decodedToken);
    Task<MedicinesAssigned?> GetByIdAsync(long id);
    Task<bool> UpdateAsync(MedicinesAssigned medicinesAssigned);
    Task<bool> DeleteAsync(long id);
    Task<bool> IsValidUserIdAsync(long userId);
    Task<bool> IsValidDoctorId(long doctorId);
    Task<bool> IsValidVisirProcedureIdAsync(long visitProcedureId);
    Task<bool> IsValidMedicineAssignedId(long id);
}
