namespace Clinic.Core.Models.Request;

public class UpdateProcedureRequest
{
    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public bool? IsActive { get; set; }
}
