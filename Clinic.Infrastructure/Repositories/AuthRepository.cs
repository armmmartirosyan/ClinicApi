using Clinic.Core.Domain;
using Clinic.Core.Interfaces.Repositories;

namespace Clinic.Infrastructure.Repositories;

public class AuthRepository(ClinicDbContext dbContext) : IAuthRepository
{
    public async Task Login()
    {
        await dbContext.UserTypes.AddAsync(new UserType() { Name = "Test"});
        await dbContext.SaveChangesAsync();
    }
}
