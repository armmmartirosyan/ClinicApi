namespace Clinic.Core.Models.Request;

public class AddVisitRequest
{
    public required long DoctorId { get; set; }
    public required DateTime StartScheduledDate { get; set; }
    public required DateTime EndScheduledDate { get; set; }
    public string? Notes { get; set; }
}
