using Blog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

public class AuthController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthController(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        var result = await _signInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, false, false);
        
        return RedirectToAction("Index", "Panel");
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        
        return RedirectToAction("Index", "Home");
    }
}