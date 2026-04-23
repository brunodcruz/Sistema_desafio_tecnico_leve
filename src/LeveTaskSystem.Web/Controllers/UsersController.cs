using System.Security.Claims;
using LeveTaskSystem.Application.DTOs;
using LeveTaskSystem.Application.Services;
using LeveTaskSystem.Domain.Enums;
using LeveTaskSystem.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeveTaskSystem.Web.Controllers;

[Authorize(Roles = nameof(UserRole.Manager))]
public class UsersController(IUserAppService userAppService, IWebHostEnvironment webHostEnvironment) : Controller
{
    private const long MaxPhotoBytes = 5 * 1024 * 1024;
    private static readonly HashSet<string> AllowedPhotoExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".gif", ".webp"
    };

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateUserViewModel());
    }

    [HttpPost]
    [RequestFormLimits(MultipartBodyLengthLimit = MaxPhotoBytes + 1024 * 1024)]
    public async Task<IActionResult> Create(CreateUserViewModel model, CancellationToken cancellationToken)
    {
        string? photoRelativePath = null;
        if (model.Photo is { Length: > 0 })
        {
            if (model.Photo.Length > MaxPhotoBytes)
            {
                ModelState.AddModelError(nameof(model.Photo), "A foto deve ter no maximo 5 MB.");
            }
            else
            {
                var ext = Path.GetExtension(model.Photo.FileName);
                if (string.IsNullOrEmpty(ext) || !AllowedPhotoExtensions.Contains(ext))
                {
                    ModelState.AddModelError(nameof(model.Photo), "Use imagem JPG, PNG, GIF ou WEBP.");
                }
                else if (!model.Photo.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(model.Photo), "Arquivo deve ser uma imagem.");
                }
            }
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (model.Photo is { Length: > 0 })
        {
            var ext = Path.GetExtension(model.Photo.FileName)!;
            var uploads = Path.Combine(webHostEnvironment.WebRootPath, "uploads", "users");
            Directory.CreateDirectory(uploads);
            var fileName = $"{Guid.NewGuid():N}{ext.ToLowerInvariant()}";
            var physicalPath = Path.Combine(uploads, fileName);
            await using (var stream = System.IO.File.Create(physicalPath))
            {
                await model.Photo.CopyToAsync(stream, cancellationToken);
            }

            photoRelativePath = $"/uploads/users/{fileName}";
        }

        var managerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        try
        {
            await userAppService.CreateUserAsync(new CreateUserDto
            {
                FullName = model.FullName,
                BirthDate = model.BirthDate,
                LandlinePhone = model.LandlinePhone,
                MobilePhone = model.MobilePhone,
                Email = model.Email,
                Address = model.Address,
                PhotoPath = photoRelativePath ?? string.Empty,
                Password = model.Password,
                Role = model.Role,
                ManagerId = model.Role == UserRole.Subordinate ? managerId : null
            }, cancellationToken);
        }
        catch (InvalidOperationException exception)
        {
            if (!string.IsNullOrEmpty(photoRelativePath))
            {
                var fileName = Path.GetFileName(photoRelativePath);
                var physical = Path.Combine(webHostEnvironment.WebRootPath, "uploads", "users", fileName);
                if (System.IO.File.Exists(physical))
                {
                    System.IO.File.Delete(physical);
                }
            }

            ModelState.AddModelError(string.Empty, exception.Message);
            return View(model);
        }

        TempData["Success"] = "Usuario cadastrado com sucesso.";
        return RedirectToAction("Index", "Home");
    }
}
