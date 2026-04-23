using LeveTaskSystem.Domain.Entities;
using LeveTaskSystem.Domain.Repositories;
using LeveTaskSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LeveTaskSystem.Infrastructure.Repositories;

public class UserRepository(LeveTaskDbContext dbContext) : IUserRepository
{
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return dbContext.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<User>> GetSubordinatesAsync(Guid managerId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.Where(x => x.ManagerId == managerId).OrderBy(x => x.FullName).ToListAsync(cancellationToken);
    }

    public Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        return dbContext.Users.AddAsync(user, cancellationToken).AsTask();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
