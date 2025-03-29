namespace Clinic.Core.Models.Request;

public class CreateNotWorkingDayRequest
{
    public required DateOnly NotWorkDate { get; set; }
}
