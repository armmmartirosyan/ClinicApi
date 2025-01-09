namespace Clinic.Core.Models.DTO;

public class UpdateNotWorkingDateValidateDTO
{
    public required long Id { get; set; }
    public required DateOnly NotWorkDate { get; set; }
}
