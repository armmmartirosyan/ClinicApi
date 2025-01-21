using Clinic.Core.Domain;

namespace Clinic.Core.Interfaces.Helpers;

public interface IAuthHelper
{
    string GenerateToken(User user);
    bool VerifyPassword(string requestPassword, string password);
    string HashPassword(string password);
}
