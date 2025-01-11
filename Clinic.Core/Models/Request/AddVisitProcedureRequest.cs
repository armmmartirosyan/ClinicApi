namespace Clinic.Core.Models.Request;

public class AddVisitProcedureRequest
{
    public long VisitId { get; set; }

    public long ProcedureId { get; set; }

    public string? Notes { get; set; }
}
