using LeveTaskSystem.Application.Services;
using LeveTaskSystem.Application.UseCases.Authentication;
using LeveTaskSystem.Application.UseCases.Tasks;
using LeveTaskSystem.Application.UseCases.Users;
using LeveTaskSystem.Domain.Repositories;
using LeveTaskSystem.Domain.Services;
using LeveTaskSystem.Infrastructure.Notifications;
using LeveTaskSystem.Infrastructure.Persistence;
using LeveTaskSystem.Infrastructure.Repositories;
using LeveTaskSystem.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LeveTaskSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LeveTaskDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.SectionName));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.AddScoped<IEmailNotifier, SmtpEmailNotifier>();

        services.AddScoped<IAuthenticateUserUseCase, AuthenticateUserUseCase>();
        services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
        services.AddScoped<ICreateTaskUseCase, CreateTaskUseCase>();
        services.AddScoped<ICompleteTaskUseCase, CompleteTaskUseCase>();

        services.AddScoped<IUserAppService, UserAppService>();
        services.AddScoped<ITaskAppService, TaskAppService>();

        return services;
    }
}
