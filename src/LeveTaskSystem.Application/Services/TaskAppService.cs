using LeveTaskSystem.Application.DTOs;
using LeveTaskSystem.Application.UseCases.Tasks;
using LeveTaskSystem.Domain.Repositories;

namespace LeveTaskSystem.Application.Services;

public class TaskAppService(
    ICreateTaskUseCase createTaskUseCase,
    ICompleteTaskUseCase completeTaskUseCase,
    ITaskRepository taskRepository) : ITaskAppService
{
    public Task CreateTaskAsync(CreateTaskDto input, CancellationToken cancellationToken = default)
    {
        return createTaskUseCase.ExecuteAsync(input, cancellationToken);
    }

    public Task CompleteTaskAsync(Guid taskId, Guid userId, CancellationToken cancellationToken = default)
    {
        return completeTaskUseCase.ExecuteAsync(taskId, userId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<TaskListItemDto>> GetTasksForManagerAsync(Guid managerId, CancellationToken cancellationToken = default)
    {
        var tasks = await taskRepository.GetTasksByManagerAsync(managerId, cancellationToken);
        return tasks.Select(t => new TaskListItemDto(
            t.Id,
            t.Message,
            t.DueDate,
            t.Status,
            t.AssignedToUser.FullName,
            t.AssignedToUser.PhotoPath ?? string.Empty,
            t.CreatedByManager.FullName,
            t.CreatedByManager.PhotoPath ?? string.Empty)).ToList();
    }

    public async Task<IReadOnlyCollection<TaskListItemDto>> GetTasksForUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tasks = await taskRepository.GetTasksByUserAsync(userId, cancellationToken);
        return tasks.Select(t => new TaskListItemDto(
            t.Id,
            t.Message,
            t.DueDate,
            t.Status,
            t.AssignedToUser.FullName,
            t.AssignedToUser.PhotoPath ?? string.Empty,
            t.CreatedByManager.FullName,
            t.CreatedByManager.PhotoPath ?? string.Empty)).ToList();
    }
}
