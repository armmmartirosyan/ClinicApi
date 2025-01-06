using Clinic.Core.Domain;

namespace Clinic.Core.Interfaces.Repositories;

public interface IVisitRepository
{
    Task<long> AddVisitAsync(Visit visit);
    Task<List<Visit>> GetAllVisitsAsync();
    Task<Visit?> GetVisitByIdAsync(long id);
    Task<bool> UpdateVisitAsync(Visit visit);
    Task<bool> DeleteVisitAsync(long id);
    Task<bool> IsValidUserIdAsync(long patientId);
    Task<VisitStatus> GetVisitStatusAsync(string name);
    Task<bool> IsValidStatusIdAsync(int? statusId);
    Task<bool> IsValidDoctorId(long doctorId);
}
