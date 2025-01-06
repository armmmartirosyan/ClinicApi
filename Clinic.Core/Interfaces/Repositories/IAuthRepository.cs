using Clinic.Core.Domain;

namespace Clinic.Core.Interfaces.Repositories;

public interface IAuthRepository
{
    Task<User> GetUserByEmailAsync(string email);
    Task<long> AddUserAsync(User user);
    Task<bool> IsValidUserTypeIdAsync(int userTypeId);
    Task<bool> IsPhoneDuplicated(string phone);
    Task<bool> IsEmailDuplicated(string email);
    //Task<bool> IsValidSpecializationIdAsync(int specializationId);
}
