using Clinic.Core.Domain;
using Clinic.Core.Models.DTO;
using Clinic.Core.Models.Request;

namespace Clinic.Core.Interfaces.Services;

public interface IVisitService
{
    Task<long> AddVisitAsync(AddVisitRequest request, long userId);
    Task<List<Visit>> GetAllVisitsAsync(DecodedTokenDTO decodedToken, long  doctorId);
    Task<Visit?> GetVisitByIdAsync(long id);
    Task<bool> UpdateVisitAsync(long id, UpdateVisitRequest request);
    Task<bool> DeleteVisitAsync(long id);
}
