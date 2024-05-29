using System.Net.Mail;
using System.Security.Claims;
using GalloFlix.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GalloFlix.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountController(
        ILogger<AccountController> logger,
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager
    )
    {
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public IActionResult Login(string returnUrl)
    {
        LoginVM login = new()
        {
            ReturnUrl = returnUrl ?? Url.Content("~/")
        };
        return View(login);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVM login)
    {
        if (ModelState.IsValid)
        {
            string userName = login.Email;
            if (IsValidEmail(userName))
            {
                var user = await _userManager.FindByEmailAsync(userName);
                if (user != null)
                    userName = user.UserName;
            }

            var result = await _signInManager.PasswordSignInAsync(
                userName, login.Password, login.RememberMe, lockoutOnFailure: true
            );

            if (result.Succeeded)
            {
                _logger.LogInformation($"Usuário {userName} fez login");
                return LocalRedirect(login.ReturnUrl);
            }

            if (result.IsLockedOut) 
            {
                _logger.LogWarning($"Usuário {userName} foi bloqueado");
                ModelState.AddModelError(string.Empty, "Conta Bloqueada! Aguarde alguns minutos para continuar!");
            }
            else
                ModelState.AddModelError(string.Empty, "Usuário e/ou Senha Inválidos!");
        }
        return View(login);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        _logger.LogInformation($"Usuario {ClaimTypes.Email} fez logoff");
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            MailAddress mail = new(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
