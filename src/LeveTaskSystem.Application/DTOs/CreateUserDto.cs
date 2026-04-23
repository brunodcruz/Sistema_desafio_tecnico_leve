using LeveTaskSystem.Domain.Enums;

namespace LeveTaskSystem.Application.DTOs;

public class CreateUserDto
{
    public string FullName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string LandlinePhone { get; set; } = string.Empty;
    public string MobilePhone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhotoPath { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public Guid? ManagerId { get; set; }
}
