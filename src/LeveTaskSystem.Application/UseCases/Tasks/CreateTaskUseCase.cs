using LeveTaskSystem.Application.DTOs;
using LeveTaskSystem.Domain.Entities;
using LeveTaskSystem.Domain.Enums;
using LeveTaskSystem.Domain.Repositories;
using LeveTaskSystem.Domain.Services;

namespace LeveTaskSystem.Application.UseCases.Tasks;

public class CreateTaskUseCase(
    ITaskRepository taskRepository,
    IUserRepository userRepository,
    IEmailNotifier emailNotifier) : ICreateTaskUseCase
{
    public async Task ExecuteAsync(CreateTaskDto input, CancellationToken cancellationToken = default)
    {
        var manager = await userRepository.GetByIdAsync(input.ManagerId, cancellationToken);
        var subordinate = await userRepository.GetByIdAsync(input.AssignedToUserId, cancellationToken);

        if (manager is null || subordinate is null)
        {
            throw new InvalidOperationException("Gestor ou subordinado nao encontrado.");
        }

        var userTask = new UserTask
        {
            Id = Guid.NewGuid(),
            Message = input.Message.Trim(),
            DueDate = input.DueDate.Date,
            Status = TaskProgressStatus.Pending,
            AssignedToUserId = subordinate.Id,
            CreatedByManagerId = manager.Id,
            CreatedAt = DateTime.UtcNow
        };

        await taskRepository.AddAsync(userTask, cancellationToken);
        await taskRepository.SaveChangesAsync(cancellationToken);

        await emailNotifier.SendAsync(
            subordinate.Email,
            "Nova tarefa atribuida",
            $"Voce recebeu uma nova tarefa com prazo em {userTask.DueDate:dd/MM/yyyy}.",
            cancellationToken);
    }
}
