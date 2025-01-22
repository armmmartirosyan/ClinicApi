namespace Clinic.Core.Models.DTO;

public class DecodedTokenDTO
{
    public long  UserId { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
}
