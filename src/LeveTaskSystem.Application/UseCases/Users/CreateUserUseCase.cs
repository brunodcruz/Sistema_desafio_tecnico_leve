using LeveTaskSystem.Application.DTOs;
using LeveTaskSystem.Domain.Entities;
using LeveTaskSystem.Domain.Enums;
using LeveTaskSystem.Domain.Repositories;
using LeveTaskSystem.Domain.Services;

namespace LeveTaskSystem.Application.UseCases.Users;

public class CreateUserUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher) : ICreateUserUseCase
{
    public async Task ExecuteAsync(CreateUserDto input, CancellationToken cancellationToken = default)
    {
        var existingUser = await userRepository.GetByEmailAsync(input.Email.Trim().ToLowerInvariant(), cancellationToken);
        if (existingUser is not null)
        {
            throw new InvalidOperationException("Ja existe um usuario com este e-mail.");
        }

        if (input.Role == UserRole.Subordinate && input.ManagerId is null)
        {
            throw new InvalidOperationException("Usuario subordinado precisa de gestor responsavel.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = input.FullName.Trim(),
            BirthDate = input.BirthDate.Date,
            LandlinePhone = input.LandlinePhone.Trim(),
            MobilePhone = input.MobilePhone.Trim(),
            Email = input.Email.Trim().ToLowerInvariant(),
            Address = input.Address.Trim(),
            PhotoPath = input.PhotoPath.Trim(),
            PasswordHash = passwordHasher.Hash(input.Password),
            Role = input.Role,
            ManagerId = input.ManagerId
        };

        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);
    }
}
