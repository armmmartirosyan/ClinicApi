namespace Clinic.Core.Models.DTO;

public class UpdateMedicinesAssignedValidateDTO
{
    public required long Id { get; set; }
    public string? Name { get; set; } = null!;
    public string? Dose { get; set; } = null!;
    public string? Notes { get; set; }
    public DateOnly? StartDate { get; set; }
    public int? Quantity { get; set; }
    public int? DayCount { get; set; }
    public long? PatientId { get; set; }
    public long? VisitProcedureId { get; set; }
}
