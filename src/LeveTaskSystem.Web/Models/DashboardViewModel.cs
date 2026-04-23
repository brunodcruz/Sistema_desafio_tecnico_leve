using LeveTaskSystem.Application.DTOs;
using LeveTaskSystem.Domain.Enums;

namespace LeveTaskSystem.Web.Models;

public class DashboardViewModel
{
    public UserRole Role { get; set; }
    public IReadOnlyCollection<TaskListItemDto> Tasks { get; set; } = [];
}
