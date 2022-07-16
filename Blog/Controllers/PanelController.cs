using Blog.Data.Repository;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Authorize(Roles = "Admin")]
public class PanelController : Controller
{
    private IRepository _repository;

    public PanelController(IRepository repository)
    {
        _repository = repository;
    }
    
    public IActionResult Index()
    {
        var posts = _repository.GetAllPosts();
        return View(posts);
    }
    
    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return View(new Post());
        }
        else
        {
            var post = _repository.GetPost((int)id);
            return View(post);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(Post? post)
    {
        if (post.Id > 0)
            _repository.UpdatePost(post);
        else
            _repository.AddPost(post);
        
        if (await _repository.SaveChangesAsync())
            return RedirectToAction("Index");
        else
            return View(post);
    }

    [HttpGet]
    public async Task<IActionResult> Remove(int id)
    {
        _repository.RemovePost(id);
        await _repository.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}