using LeveTaskSystem.Application.DTOs;
using LeveTaskSystem.Application.UseCases.Authentication;
using LeveTaskSystem.Application.UseCases.Users;

namespace LeveTaskSystem.Application.Services;

public class UserAppService(
    IAuthenticateUserUseCase authenticateUserUseCase,
    ICreateUserUseCase createUserUseCase) : IUserAppService
{
    public Task<AuthResultDto?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        return authenticateUserUseCase.ExecuteAsync(email, password, cancellationToken);
    }

    public Task CreateUserAsync(CreateUserDto input, CancellationToken cancellationToken = default)
    {
        return createUserUseCase.ExecuteAsync(input, cancellationToken);
    }
}
