using LeveTaskSystem.Domain.Enums;

namespace LeveTaskSystem.Domain.Entities;

public class UserTask
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public TaskProgressStatus Status { get; set; }
    public Guid AssignedToUserId { get; set; }
    public User AssignedToUser { get; set; } = null!;
    public Guid CreatedByManagerId { get; set; }
    public User CreatedByManager { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
