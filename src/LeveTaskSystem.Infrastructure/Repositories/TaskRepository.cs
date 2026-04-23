using LeveTaskSystem.Domain.Entities;
using LeveTaskSystem.Domain.Repositories;
using LeveTaskSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LeveTaskSystem.Infrastructure.Repositories;

public class TaskRepository(LeveTaskDbContext dbContext) : ITaskRepository
{
    public Task<UserTask?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.Tasks
            .Include(x => x.AssignedToUser)
            .Include(x => x.CreatedByManager)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<UserTask>> GetTasksByManagerAsync(Guid managerId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Tasks
            .Include(x => x.AssignedToUser)
            .Include(x => x.CreatedByManager)
            .Where(x => x.CreatedByManagerId == managerId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<UserTask>> GetTasksByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Tasks
            .Include(x => x.AssignedToUser)
            .Include(x => x.CreatedByManager)
            .Where(x => x.AssignedToUserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task AddAsync(UserTask userTask, CancellationToken cancellationToken = default)
    {
        return dbContext.Tasks.AddAsync(userTask, cancellationToken).AsTask();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
