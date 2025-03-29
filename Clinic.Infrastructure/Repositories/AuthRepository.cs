using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Clinic.Core.Models.DTO;
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
    
    public async Task<InfiniteScrollDTO<User>> GetDoctors(int page, int pageSize, long userId)
    {
        var doctorType = await dbContext.UserTypes.FirstOrDefaultAsync(ut => ut.Name == "Doctor");
        var totalItems = await dbContext.Users.CountAsync();
        bool allowNext = (page * pageSize) < totalItems;
        
        List<User> doctors = await dbContext.Users
            .Where(u => u.TypesId == doctorType.Id && u.Id != userId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Join(dbContext.Users, u => u.Id, ds => ds.Id, (u, _) =>  new User()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                CreatedAt = u.CreatedAt,
                Email = u.Email,
                NotWorkingDays = u.NotWorkingDays,
                Types = u.Types,
                WeekDaySchedules = u.WeekDaySchedules,
                Phone = u.Phone,
                ImageUrl = u.ImageUrl,
                TypesId = u.TypesId,
                BirthDate = u.BirthDate
            })
            .ToListAsync();
        
        foreach (var doctor in doctors)
        {
            var doctorSpecializations = await dbContext.DoctorsSpecializations
                .Where(s => s.DoctorId == doctor.Id)
                .Join(dbContext.DoctorsSpecializations, u => u.DoctorId, ds => ds.DoctorId, (u, _) =>  new DoctorsSpecialization
                {
                    DoctorId = doctor.Id,
                    SpecializationId = u.SpecializationId,
                    Specialization = u.Specialization
                })
                .ToListAsync();

            doctor.DoctorsSpecializations = doctorSpecializations;
        }

        return new InfiniteScrollDTO<User>()
        {
            AllowNext = allowNext,
            Data = doctors
        };
    }
    
    public async Task<User> GetProfileAsync(DecodedTokenDTO decodedToken)
    {
        var user = await dbContext.Users
            .Join(dbContext.Users, u => u.Id, ds => ds.Id, (u, _) => new User()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                CreatedAt = u.CreatedAt,
                Email = u.Email,
                NotWorkingDays = u.NotWorkingDays,
                Types = u.Types,
                WeekDaySchedules = u.WeekDaySchedules,
                Phone = u.Phone,
                ImageUrl = u.ImageUrl,
                TypesId = u.TypesId,
                BirthDate = u.BirthDate
            })
            .FirstOrDefaultAsync(u => u.Id == decodedToken.UserId);

        if (decodedToken.Role == "Doctor")
        {
            var doctorSpecializations = await dbContext.DoctorsSpecializations
                .Where(s => s.DoctorId == user.Id)
                .Join(dbContext.DoctorsSpecializations, u => u.DoctorId, ds => ds.DoctorId, (u, _) =>  new DoctorsSpecialization
                {
                    DoctorId = user.Id,
                    SpecializationId = u.SpecializationId,
                    Specialization = u.Specialization
                })
                .ToListAsync();

            user.DoctorsSpecializations = doctorSpecializations;
        }

        return user;
    }
    
    public async Task<User> GetUserByIdAsync(long id)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
    
    public async Task<bool> UpdateProfileAsync(User user)
    {
        dbContext.Users.Update(user);
        return await dbContext.SaveChangesAsync() > 0;
    }
}