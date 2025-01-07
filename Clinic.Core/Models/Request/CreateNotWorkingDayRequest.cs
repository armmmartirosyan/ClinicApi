namespace Clinic.Core.Models.Request;

public class CreateNotWorkingDayRequest
{
    public required long DoctorId { get; set; }

    public required DateOnly NotWorkDate { get; set; }
}
