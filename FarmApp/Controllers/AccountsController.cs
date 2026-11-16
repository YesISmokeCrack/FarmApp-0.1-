using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmApp.Controllers;

[ApiController]
[Route("api/accounts")]
[Authorize(Roles = AuthController.AdminRole)]
public sealed class AccountsController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;

    public AccountsController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetWorkers()
    {
        var users = await _userManager.Users.OrderBy(user => user.UserName).ToListAsync();
        var workers = new List<object>();

        foreach (var user in users)
        {
            if (await _userManager.IsInRoleAsync(user, AuthController.WorkerRole))
            {
                workers.Add(new { id = user.Id, userName = user.UserName, role = AuthController.WorkerRole });
            }
        }

        return Ok(workers);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWorker(CreateWorkerRequest request)
    {
        var worker = new IdentityUser { UserName = request.UserName.Trim() };
        var createResult = await _userManager.CreateAsync(worker, request.Password);
        if (!createResult.Succeeded)
        {
            return BadRequest(new { message = string.Join(" ", createResult.Errors.Select(AuthController.TranslateIdentityError)) });
        }

        var roleResult = await _userManager.AddToRoleAsync(worker, AuthController.WorkerRole);
        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(worker);
            return BadRequest(new { message = string.Join(" ", roleResult.Errors.Select(AuthController.TranslateIdentityError)) });
        }

        return CreatedAtAction(nameof(GetWorkers), new { id = worker.Id },
            new { id = worker.Id, userName = worker.UserName, role = AuthController.WorkerRole });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorker(string id)
    {
        var worker = await _userManager.FindByIdAsync(id);
        if (worker is null || !await _userManager.IsInRoleAsync(worker, AuthController.WorkerRole))
        {
            return NotFound();
        }

        var result = await _userManager.DeleteAsync(worker);
        if (!result.Succeeded)
        {
            return BadRequest(new { message = string.Join(" ", result.Errors.Select(error => error.Description)) });
        }

        return NoContent();
    }

    public sealed class CreateWorkerRequest
    {
        [Required]
        [StringLength(32, MinimumLength = 3)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}
