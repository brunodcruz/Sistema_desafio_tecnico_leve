using LeveTaskSystem.Application.DTOs;

namespace LeveTaskSystem.Application.Services;

public interface ITaskAppService
{
    Task CreateTaskAsync(CreateTaskDto input, CancellationToken cancellationToken = default);
    Task CompleteTaskAsync(Guid taskId, Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TaskListItemDto>> GetTasksForManagerAsync(Guid managerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TaskListItemDto>> GetTasksForUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
