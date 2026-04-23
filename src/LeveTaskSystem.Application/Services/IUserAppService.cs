using LeveTaskSystem.Application.DTOs;

namespace LeveTaskSystem.Application.Services;

public interface IUserAppService
{
    Task<AuthResultDto?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default);
    Task CreateUserAsync(CreateUserDto input, CancellationToken cancellationToken = default);
}
