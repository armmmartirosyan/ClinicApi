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
            
            Console.Write($"doctorSpecializations: {doctorSpecializations}:");

            doctor.DoctorsSpecializations = doctorSpecializations;
        }

        return new InfiniteScrollDTO<User>()
        {
            AllowNext = allowNext,
            Data = doctors
        };
    }
}
/*
.Join(dbContext.DoctorsSpecializations, u => u.Id, ds => ds.DoctorId, (u, ds) => new User()
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
       DoctorsSpecializations = u.DoctorsSpecializations,
   })


.Join(dbContext.DoctorsSpecializations, u => u.Id, ds => ds.DoctorId, (u, ds) => new  { u, ds })
.Join(dbContext.Specializations, ur => ur.ds.Specialization, r => r.Id, (ur, r) => new User()
   {
       Id = ur.u.Id,
       FirstName = ur.u.FirstName,
       LastName = ur.u.LastName,
       CreatedAt = ur.u.CreatedAt,
       Email = ur.u.Email,
       NotWorkingDays = ur.u.NotWorkingDays,
       DoctorsSpecializations = ur.u.DoctorsSpecializations,
       Types = ur.u.Types,
       WeekDaySchedules = ur.u.WeekDaySchedules,
       Phone = ur.u.Phone,
       ImageUrl = ur.u.ImageUrl,
       TypesId = ur.u.TypesId,
   })

*/