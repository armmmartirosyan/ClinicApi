using Clinic.Core.Domain;

namespace Clinic.Core.Interfaces.Repositories;

public interface ISpecializationsRepository
{
    Task<int> AddAsync(Specialization specialization);
    Task<List<Specialization>> GetAllAsync();
    Task<Specialization?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(Specialization specialization);
    Task<bool> DeleteAsync(int id);
    Task<bool> NameNotDuplicated(string name);
    Task<Specialization?> GetByNameAsync(string name);
}
