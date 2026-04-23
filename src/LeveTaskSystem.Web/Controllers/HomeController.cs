using System.Security.Claims;
using LeveTaskSystem.Application.Services;
using LeveTaskSystem.Domain.Enums;
using LeveTaskSystem.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeveTaskSystem.Web.Controllers;

[Authorize]
public class HomeController(ITaskAppService taskAppService) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = Enum.Parse<UserRole>(User.FindFirstValue(ClaimTypes.Role)!);

        var tasks = role == UserRole.Manager
            ? await taskAppService.GetTasksForManagerAsync(userId, cancellationToken)
            : await taskAppService.GetTasksForUserAsync(userId, cancellationToken);

        return View(new DashboardViewModel
        {
            Role = role,
            Tasks = tasks
        });
    }
}
