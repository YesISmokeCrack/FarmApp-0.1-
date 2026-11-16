using System.ComponentModel.DataAnnotations;
using FarmApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmApp.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    public const string AdminRole = "Администратор";
    public const string WorkerRole = "Работник";
    private static readonly SemaphoreSlim SetupLock = new(1, 1);

    private readonly AuthDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthController(
        AuthDbContext context,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<IdentityUser> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }

    [AllowAnonymous]
    [HttpGet("setup-status")]
    public async Task<IActionResult> GetSetupStatus()
    {
        return Ok(new { setupRequired = !await _userManager.Users.AnyAsync() });
    }

    [AllowAnonymous]
    [HttpPost("setup")]
    public async Task<IActionResult> Setup(SetupAdminRequest request)
    {
        await SetupLock.WaitAsync();
        try
        {
            if (await _userManager.Users.AnyAsync())
            {
                return Conflict(new { message = "Первичная настройка уже выполнена. Войдите в систему." });
            }

            var adminRole = await EnsureRoleExists(AdminRole);
            if (!adminRole.Succeeded)
            {
                return IdentityError(adminRole);
            }

            var workerRole = await EnsureRoleExists(WorkerRole);
            if (!workerRole.Succeeded)
            {
                return IdentityError(workerRole);
            }

            var admin = new IdentityUser { UserName = request.UserName.Trim() };
            var createResult = await _userManager.CreateAsync(admin, request.Password);
            if (!createResult.Succeeded)
            {
                return IdentityError(createResult);
            }

            var roleResult = await _userManager.AddToRoleAsync(admin, AdminRole);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(admin);
                return IdentityError(roleResult);
            }

            await _signInManager.SignInAsync(admin, isPersistent: false);
            return Ok(new { userName = admin.UserName, role = AdminRole, isAdmin = true });
        }
        finally
        {
            SetupLock.Release();
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName.Trim());
        if (user is null)
        {
            return Unauthorized(new { message = "Неверный логин или пароль." });
        }

        var result = await _signInManager.PasswordSignInAsync(
            user,
            request.Password,
            isPersistent: false,
            lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            return StatusCode(StatusCodes.Status423Locked,
                new { message = "Слишком много попыток входа. Попробуйте через 15 минут." });
        }

        if (!result.Succeeded)
        {
            return Unauthorized(new { message = "Неверный логин или пароль." });
        }

        var isAdmin = await _userManager.IsInRoleAsync(user, AdminRole);
        return Ok(new { userName = user.UserName, role = isAdmin ? AdminRole : WorkerRole, isAdmin });
    }

    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        return Ok(new
        {
            userName = User.Identity?.Name,
            isAdmin = User.IsInRole(AdminRole),
            role = User.IsInRole(AdminRole) ? AdminRole : WorkerRole
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return NoContent();
    }

    private async Task<IdentityResult> EnsureRoleExists(string roleName)
    {
        if (await _roleManager.RoleExistsAsync(roleName))
        {
            return IdentityResult.Success;
        }

        return await _roleManager.CreateAsync(new IdentityRole(roleName));
    }

    private IActionResult IdentityError(IdentityResult result)
    {
        return BadRequest(new
        {
            message = string.Join(" ", result.Errors.Select(TranslateIdentityError))
        });
    }

    public static string TranslateIdentityError(IdentityError error)
    {
        return error.Code switch
        {
            "PasswordTooShort" => "Пароль должен содержать не менее 8 символов.",
            "PasswordRequiresDigit" => "Добавьте в пароль хотя бы одну цифру.",
            "PasswordRequiresUpper" => "Добавьте заглавную латинскую букву.",
            "PasswordRequiresLower" => "Добавьте строчную латинскую букву.",
            "DuplicateUserName" => "Этот логин уже занят. Выберите другой.",
            "InvalidUserName" => "Логин может содержать латинские буквы, цифры и символы . _ - @ +.",
            _ => error.Description
        };
    }

    public sealed class SetupAdminRequest
    {
        [Required]
        [StringLength(32, MinimumLength = 3)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }

    public sealed class LoginRequest
    {
        [Required]
        [StringLength(32, MinimumLength = 3)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}
