using System.ComponentModel.DataAnnotations;

namespace LeveTaskSystem.Web.Models;

public class CreateTaskViewModel
{
    [Required]
    public Guid AssignedToUserId { get; set; }

    [Required]
    public string Message { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime DueDate { get; set; }
}
