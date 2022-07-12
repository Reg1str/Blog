using Blog.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Post()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Edit()
    {
        return View(new Post());
    }
    
    [HttpPost]
    public IActionResult Edit(Post post)
    {
        
        return RedirectToAction("Index");
    }

}