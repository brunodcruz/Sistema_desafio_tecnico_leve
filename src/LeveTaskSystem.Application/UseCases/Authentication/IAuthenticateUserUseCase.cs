using LeveTaskSystem.Application.DTOs;

namespace LeveTaskSystem.Application.UseCases.Authentication;

public interface IAuthenticateUserUseCase
{
    Task<AuthResultDto?> ExecuteAsync(string email, string password, CancellationToken cancellationToken = default);
}
