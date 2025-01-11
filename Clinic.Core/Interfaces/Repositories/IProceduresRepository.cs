using Clinic.Core.Domain;

namespace Clinic.Core.Interfaces.Repositories;

public interface IProceduresRepository
{
    Task<long> AddProcedureAsync(Procedure procedure);
    Task<List<Procedure>> GetAllProceduresAsync();
    Task<Procedure?> GetProcedureByIdAsync(long id);
    Task<bool> UpdateProcedureAsync(Procedure procedure);
    Task<bool> DeleteProcedureAsync(long id);
    Task<bool> ProcedureNameNotDuplicated(string name);
    Task<Procedure?> GetProcedureByNameAsync(string name);
}
