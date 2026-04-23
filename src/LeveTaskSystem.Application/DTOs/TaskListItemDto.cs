using LeveTaskSystem.Domain.Enums;

namespace LeveTaskSystem.Application.DTOs;

public record TaskListItemDto(
    Guid Id,
    string Message,
    DateTime DueDate,
    TaskProgressStatus Status,
    string AssignedToName,
    string AssignedToPhotoPath,
    string CreatedByName,
    string CreatedByPhotoPath
);
