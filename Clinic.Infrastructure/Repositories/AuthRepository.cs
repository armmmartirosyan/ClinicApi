using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Repositories;

public class AuthRepository(ClinicDbContext dbContext) : IAuthRepository
{
    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<long> AddUserAsync(User user)
    {
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        User currentUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

        return currentUser.Id;
    }

    public async Task<bool> IsValidUserTypeIdAsync(int userTypeId)
    {
        return await dbContext.UserTypes.AnyAsync(ut => ut.Id == userTypeId);
    }

    public async Task<bool> IsPhoneDuplicated(string phone)
    {
        return await dbContext.Users.AnyAsync(ut => ut.Phone == phone);
    }

    public async Task<bool> IsEmailDuplicated(string email)
    {
        return await dbContext.Users.AnyAsync(ut => ut.Email == email);
    }

    //public async Task<bool> IsValidSpecializationIdAsync(int specializationId)
    //{
    //    return await dbContext.Specializations
    //        .AnyAsync(s => s.Id == specializationId);
    //}
}
