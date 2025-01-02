namespace Clinic.Core.Models.Request;

public class RegisterRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public int TypesId { get; set; }
    public DateOnly BirthDate { get; set; } 
    public List<int>? Specializations { get; set; }
}
