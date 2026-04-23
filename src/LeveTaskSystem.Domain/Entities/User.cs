using LeveTaskSystem.Domain.Enums;

namespace LeveTaskSystem.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string LandlinePhone { get; set; } = string.Empty;
    public string MobilePhone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhotoPath { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public Guid? ManagerId { get; set; }
    public User? Manager { get; set; }
    public ICollection<UserTask> AssignedTasks { get; set; } = new List<UserTask>();
    public ICollection<UserTask> CreatedTasks { get; set; } = new List<UserTask>();
}
