using Clinic.Core.Domain;

namespace Clinic.Core.Interfaces.Repositories;

public interface IAuthRepository
{
    Task<User> GetUserByEmailAsync(string email);
    Task<long> AddUserAsync(User user);
}
