using Clinic.Core.Domain;
using Clinic.Core.Models.Request;

namespace Clinic.Core.Interfaces.Services;

public interface IMedicinesAssignedService
{
    Task<long> AddAsync(AddMedicinesAssignedRequest request);
    Task<MedicinesAssigned> GetByIdAsync(long id);
    Task<IEnumerable<MedicinesAssigned>> GetAllAsync();
    Task<bool> UpdateAsync(long id, UpdateMedicinesAssignedRequest request);
    Task<bool> DeleteAsync(long id);
}
