using System.Net;
using Blog.Services.Email;
using Blog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

public class AuthController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    //private readonly IEmailService _emailService;

    public AuthController(SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager)
        //IEmailService emailService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        //_emailService = emailService;
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
        if (!result.Succeeded)
        {
            return View(viewModel);
        }

        var user = await _userManager.FindByNameAsync(viewModel.UserName);
        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
        if (isAdmin)
        { 
            return RedirectToAction("Index", "Panel");  
        }
        
        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }
        
        var user = new IdentityUser
        {
            UserName = viewModel.Email,
            Email = viewModel.Email
        };
        var result = await _userManager.CreateAsync(user, "password");
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            //await _emailService.SendEmail(user.Email, "Welcome", "Thank you for registering!");
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        
        return RedirectToAction("Index", "Home");
    }
}