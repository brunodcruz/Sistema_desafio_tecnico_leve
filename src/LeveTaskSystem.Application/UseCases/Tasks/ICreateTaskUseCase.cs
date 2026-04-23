using LeveTaskSystem.Application.DTOs;

namespace LeveTaskSystem.Application.UseCases.Tasks;

public interface ICreateTaskUseCase
{
    Task ExecuteAsync(CreateTaskDto input, CancellationToken cancellationToken = default);
}
