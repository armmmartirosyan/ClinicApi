using Clinic.Core.Domain;
using Clinic.Core.Models.Request;

namespace Clinic.Core.Interfaces.Services;

public interface ISpecializationsService
{
    Task<int> AddAsync(AddUpdateSpecializationRequest request);
    Task<Specialization> GetByIdAsync(int id);
    Task<IEnumerable<Specialization>> GetAllAsync();
    Task<bool> UpdateAsync(int id, AddUpdateSpecializationRequest request);
    Task DeleteAsync(int id);
}
