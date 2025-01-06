namespace Clinic.Core.Models.Request;

public class UpdateVisitRequest
{
    public DateTime? StartScheduledDate { get; set; }
    public DateTime? EndScheduledDate { get; set; }
    public DateTime? StartActualDate { get; set; }
    public DateTime? EndActualDate { get; set; }
    public int? StatusId { get; set; }
    public string? Notes { get; set; }
}
