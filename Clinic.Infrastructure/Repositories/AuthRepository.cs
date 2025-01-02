using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Repositories;

public class AuthRepository(ClinicDbContext dbContext) : IAuthRepository
{
    public async Task<User> GetUserByEmailAsync(string email)
    {
        // JUST EXAMPLE
        //await dbContext.UserTypes.AddAsync(new UserType() { Name = "Test" });
        //await dbContext.SaveChangesAsync();

        return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}
