using Microsoft.AspNetCore.Http;

namespace Clinic.Core.Models.Request;

public class AddVisitProcedureRequest
{
    public long VisitId { get; set; }

    public long ProcedureId { get; set; }

    public string? Notes { get; set; }

    public List<IFormFile>? Images { get; set; }
}
