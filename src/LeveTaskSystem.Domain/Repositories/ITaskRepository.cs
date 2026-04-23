using LeveTaskSystem.Domain.Entities;

namespace LeveTaskSystem.Domain.Repositories;

public interface ITaskRepository
{
    Task<UserTask?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<UserTask>> GetTasksByManagerAsync(Guid managerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<UserTask>> GetTasksByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(UserTask userTask, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
