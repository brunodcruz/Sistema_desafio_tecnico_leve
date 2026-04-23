using LeveTaskSystem.Application.DTOs;

namespace LeveTaskSystem.Application.UseCases.Users;

public interface ICreateUserUseCase
{
    Task ExecuteAsync(CreateUserDto input, CancellationToken cancellationToken = default);
}
