using Microsoft.AspNetCore.Http;

namespace Clinic.Core.Models.Request;

public class UploadProcedureImagesRequest
{
    public long VisitProcedureId { get; set; }
    public required List<IFormFile> Images { get; set; }
}
