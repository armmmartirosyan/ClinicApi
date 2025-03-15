using Clinic.Core.Domain;
using Clinic.Core.Models.Request;
using Clinic.Core.Models.DTO;

namespace Clinic.Core.Interfaces.Services;

public interface IMedicinesAssignedService
{
    Task<long> AddAsync(AddMedicinesAssignedRequest request);
    Task<MedicinesAssigned> GetByIdAsync(long id);
    Task<InfiniteScrollDTO<MedicinesAssigned>> GetAllAsync(int page, int pageSize);
    Task<bool> UpdateAsync(long id, UpdateMedicinesAssignedRequest request);
    Task<bool> DeleteAsync(long id);
}
