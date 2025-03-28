﻿namespace Clinic.Core.Models.Request;

public class RegisterRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Phone { get; set; }
    public required int TypesId { get; set; }
    public required DateOnly BirthDate { get; set; } 
    public List<int>? Specializations { get; set; }
}
