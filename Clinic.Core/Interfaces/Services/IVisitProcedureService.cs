using Clinic.Core.Domain;
using Clinic.Core.Models.Request;
using Clinic.Core.Models.DTO;

namespace Clinic.Core.Interfaces.Services;

public interface IVisitProcedureService
{
    Task<long> AddAsync(AddVisitProcedureRequest request);
    Task<VisitsProcedure> GetByIdAsync(long id);
    Task<InfiniteScrollDTO<VisitsProcedure>> GetAllAsync(int page, int pageSize);
    Task<bool> UpdateAsync(long id, UpdateVisitProcedureRequest request);
    Task<bool> DeleteAsync(long id);
    Task<bool> DeleteImageByUrlAsync(string url);
    Task UploadVisitProcedureImagesAsync(UploadProcedureImagesRequest request);
}
