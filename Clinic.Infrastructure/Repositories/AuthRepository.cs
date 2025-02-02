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

    public async Task<User> GetUserWithRoleByEmailAsync(string email)
    {
        return await dbContext.Users.Where(u => u.Email == email).Join(dbContext.Users, u => u.Id, u => u.Id, (u, _) => new User
        {
            Id = u.Id,
            Email = email,
            TypesId = u.TypesId,
            Password = u.Password,
            Types = u.Types, 
        }).FirstOrDefaultAsync();
    }

public async Task<long> AddUserAsync(User user)
    {
        var entry = await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return entry.Entity.Id;
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

    public async Task<bool> IsValidSpecializationIdAsync(int specializationId)
    {
        return await dbContext.Specializations
            .AnyAsync(s => s.Id == specializationId);
    }

    public async Task<bool> AssignSpecializationsToUser(List<DoctorsSpecialization> doctorSpecializations)
    {   
        await dbContext.DoctorsSpecializations.AddRangeAsync(doctorSpecializations);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> IsDoctorTypeId(int id)
    {
        return await dbContext.UserTypes.AnyAsync(ut => ut.Id == id && ut.Name == "Doctor");
    }
    
    public async Task<UserType?> GetUserTypeByName(string name)
    {
        return await dbContext.UserTypes.FirstOrDefaultAsync(ut => ut.Name == name);
    }
}
