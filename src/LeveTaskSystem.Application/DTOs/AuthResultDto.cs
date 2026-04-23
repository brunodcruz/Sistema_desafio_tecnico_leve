using LeveTaskSystem.Domain.Enums;

namespace LeveTaskSystem.Application.DTOs;

public record AuthResultDto(Guid UserId, string FullName, string Email, UserRole Role);
