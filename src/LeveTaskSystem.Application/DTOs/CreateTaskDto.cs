namespace LeveTaskSystem.Application.DTOs;

public class CreateTaskDto
{
    public Guid ManagerId { get; set; }
    public Guid AssignedToUserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
}
