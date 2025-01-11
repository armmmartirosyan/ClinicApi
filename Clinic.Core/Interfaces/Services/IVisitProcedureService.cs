using Clinic.Core.Domain;
using Clinic.Core.Models.Request;

namespace Clinic.Core.Interfaces.Services;

public interface IVisitProcedureService
{
    Task<long> AddAsync(AddVisitProcedureRequest request);
    Task<VisitsProcedure> GetByIdAsync(long id);
    Task<IEnumerable<VisitsProcedure>> GetAllAsync();
    Task<bool> UpdateAsync(long id, UpdateVisitProcedureRequest request);
    Task DeleteAsync(long id);
}
