using Clinic.Core.Domain;
using Clinic.Core.Models.Request;

namespace Clinic.Core.Interfaces.Services;

public interface IProceduresService
{
    Task<long> AddAsync(AddProcedureRequest request);
    Task<Procedure> GetByIdAsync(long id);
    Task<IEnumerable<Procedure>> GetProceduresAsync();
    Task<bool> UpdateAsync(long id, UpdateProcedureRequest request);
    Task DeleteAsync(long id);
}
