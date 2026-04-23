using System.Security.Claims;
using LeveTaskSystem.Application.DTOs;
using LeveTaskSystem.Application.Services;
using LeveTaskSystem.Domain.Enums;
using LeveTaskSystem.Domain.Repositories;
using LeveTaskSystem.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeveTaskSystem.Web.Controllers;

[Authorize]
public class TasksController(ITaskAppService taskAppService, IUserRepository userRepository) : Controller
{
    [Authorize(Roles = nameof(UserRole.Manager))]
    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await LoadSubordinatesAsync(cancellationToken);
        return View(new CreateTaskViewModel { DueDate = DateTime.Today.AddDays(1) });
    }

    [Authorize(Roles = nameof(UserRole.Manager))]
    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await LoadSubordinatesAsync(cancellationToken);
            return View(model);
        }

        var managerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        try
        {
            await taskAppService.CreateTaskAsync(new CreateTaskDto
            {
                ManagerId = managerId,
                AssignedToUserId = model.AssignedToUserId,
                Message = model.Message,
                DueDate = model.DueDate
            }, cancellationToken);
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            await LoadSubordinatesAsync(cancellationToken);
            return View(model);
        }

        TempData["Success"] = "Tarefa cadastrada com sucesso.";
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Complete(Guid taskId, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await taskAppService.CompleteTaskAsync(taskId, userId, cancellationToken);
        return RedirectToAction("Index", "Home");
    }

    private async Task LoadSubordinatesAsync(CancellationToken cancellationToken)
    {
        var managerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var subordinates = await userRepository.GetSubordinatesAsync(managerId, cancellationToken);
        ViewBag.Subordinates = subordinates
            .Select(x => new SelectListItem(x.FullName, x.Id.ToString()))
            .ToList();
    }
}
