using GymManagementSystem.Application.DTOs;
using GymManagementSystem.Application.Interfaces.Persistence;
using GymManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GymManagementSystem.Api.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IUnitOfWork _uow;

    public AccountController(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IUnitOfWork uow)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _uow = uow;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            // 1. Create the Gym (Tenant)
            var gym = new Gym 
            { 
                Name = model.GymName, 
                Address = model.GymAddress 
            };
            await _uow.Gyms.AddAsync(gym);
            await _uow.SaveChangesAsync(); // Get the CompanyId

            // 2. Create the User (Gym Admin)
            var user = new ApplicationUser 
            { 
                UserName = model.Email, 
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CompanyId = gym.Id // Link to tenant
            };
            
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // 3. Ensure GymAdmin role exists and assign it
                if (!await _roleManager.RoleExistsAsync("GymAdmin"))
                    await _roleManager.CreateAsync(new IdentityRole<Guid>("GymAdmin"));
                
                await _userManager.AddToRoleAsync(user, "GymAdmin");

                // 4. Add CompanyId claim for multi-tenancy logic
                await _userManager.AddClaimAsync(user, new Claim("CompanyId", gym.Id.ToString()));

                // 5. Sign in
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            
            // Cleanup: If user creation failed, we should ideally rollback Gym creation.
            // Simplified for this demo.
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult AccessDenied() => View();
}
