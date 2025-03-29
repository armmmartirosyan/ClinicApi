namespace Clinic.Core.Models.Request;

public class CreateNotWorkingDayRequestValidator
{
    public required long DoctorId { get; set; }
    
    public required DateOnly NotWorkDate { get; set; }
}
