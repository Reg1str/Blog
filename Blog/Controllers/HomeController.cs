using Blog.Data;
using Blog.Data.Repository;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;
public class HomeController : Controller
{
    private IRepository _repository;

    public HomeController(IRepository repository)
    {
        _repository = repository;
    }
    
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
    public async Task<IActionResult> Edit(Post post)
    {
        _repository.AddPost(post);
        if (await _repository.SaveChangesAsync())
            return RedirectToAction("Index");
        else
            return View(post);
    }

}