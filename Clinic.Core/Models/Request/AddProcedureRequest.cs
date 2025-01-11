namespace Clinic.Core.Models.Request;

public class AddProcedureRequest
{
    public required string Name { get; set; }

    public required decimal Price { get; set; }

    public bool? IsActive { get; set; }
}
