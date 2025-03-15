using Clinic.Core.Domain;
using Clinic.Core.Models.DTO;

namespace Clinic.Core.Interfaces.Repositories;

public interface IAuthRepository
{
    Task<User> GetUserByEmailAsync(string email);
    Task<long> AddUserAsync(User user);
    Task<bool> IsValidUserTypeIdAsync(int userTypeId);
    Task<bool> IsPhoneDuplicated(string phone);
    Task<bool> IsEmailDuplicated(string email);
    Task<bool> IsValidSpecializationIdAsync(int specializationId);
    Task<bool> AssignSpecializationsToUser(List<DoctorsSpecialization> specializations);
    Task<bool> IsDoctorTypeId(int id);
    Task<User> GetUserWithRoleByEmailAsync(string email);
    Task<UserType?> GetUserTypeByName(string name);
    Task<InfiniteScrollDTO<User>> GetDoctors(int page, int pageSize);
}
