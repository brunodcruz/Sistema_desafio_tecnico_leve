using System.ComponentModel.DataAnnotations;
using LeveTaskSystem.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace LeveTaskSystem.Web.Models;

public class CreateUserViewModel
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }

    [Required]
    public string LandlinePhone { get; set; } = string.Empty;

    [Required]
    public string MobilePhone { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    [Display(Name = "Foto")]
    public IFormFile? Photo { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public UserRole Role { get; set; }

    public Guid? ManagerId { get; set; }
}
