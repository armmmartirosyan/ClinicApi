namespace Clinic.Core.Models.Request;

public class UpdateProfileRequest
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Phone { get; set; }

    public DateOnly BirthDate { get; set; }

}
