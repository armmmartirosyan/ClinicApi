using Clinic.Core.Domain;
using Clinic.Core.Models.Request;
using Clinic.Core.Models.DTO;

namespace Clinic.Core.Interfaces.Services;

public interface IAuthService
{
    Task<string> SignInAsync(SignInRequest request);
    Task<string> RegisterAsync(RegisterRequest request);
    Task<UserType?> GetUserTypeByName(string name);
    Task<InfiniteScrollDTO<User>> GetDoctors(int page, int pageSize, long userId);
    Task<User> GetProfileAsync(DecodedTokenDTO decodedToken);
    Task<bool> UpdateProfileAsync(DecodedTokenDTO decodedToken, UpdateProfileRequest request);
}
