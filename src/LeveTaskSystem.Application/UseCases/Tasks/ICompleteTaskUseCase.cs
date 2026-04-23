namespace LeveTaskSystem.Application.UseCases.Tasks;

public interface ICompleteTaskUseCase
{
    Task ExecuteAsync(Guid taskId, Guid userId, CancellationToken cancellationToken = default);
}
