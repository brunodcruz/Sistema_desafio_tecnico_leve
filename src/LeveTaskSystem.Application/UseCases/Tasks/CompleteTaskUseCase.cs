using LeveTaskSystem.Domain.Enums;
using LeveTaskSystem.Domain.Repositories;
using LeveTaskSystem.Domain.Services;

namespace LeveTaskSystem.Application.UseCases.Tasks;

public class CompleteTaskUseCase(
    ITaskRepository taskRepository,
    IUserRepository userRepository,
    IEmailNotifier emailNotifier) : ICompleteTaskUseCase
{
    public async Task ExecuteAsync(Guid taskId, Guid userId, CancellationToken cancellationToken = default)
    {
        var userTask = await taskRepository.GetByIdAsync(taskId, cancellationToken);
        if (userTask is null || userTask.AssignedToUserId != userId)
        {
            throw new InvalidOperationException("Tarefa nao encontrada para o usuario.");
        }

        if (userTask.Status == TaskProgressStatus.Completed)
        {
            return;
        }

        userTask.Status = TaskProgressStatus.Completed;
        userTask.CompletedAt = DateTime.UtcNow;
        await taskRepository.SaveChangesAsync(cancellationToken);

        var manager = await userRepository.GetByIdAsync(userTask.CreatedByManagerId, cancellationToken);
        var subordinate = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (manager is not null && subordinate is not null)
        {
            await emailNotifier.SendAsync(
                manager.Email,
                "Tarefa concluida.",
                $"{subordinate.FullName} Concluiu a tarefa: {userTask.Message}",
                cancellationToken);
        }
    }
}
