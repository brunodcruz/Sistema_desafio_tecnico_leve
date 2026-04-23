using LeveTaskSystem.Application.DTOs;
using LeveTaskSystem.Domain.Repositories;
using LeveTaskSystem.Domain.Services;

namespace LeveTaskSystem.Application.UseCases.Authentication;

public class AuthenticateUserUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher) : IAuthenticateUserUseCase
{
    public async Task<AuthResultDto?> ExecuteAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByEmailAsync(email.Trim().ToLowerInvariant(), cancellationToken);
        if (user is null || !passwordHasher.Verify(password, user.PasswordHash))
        {
            return null;
        }

        return new AuthResultDto(user.Id, user.FullName, user.Email, user.Role);
    }
}
