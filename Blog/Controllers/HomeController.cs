using Blog.Data.FileManager;
using Blog.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;
public class HomeController : Controller
{
    private IRepository _repository;
    private IFileManager _fileManager;

    public HomeController(IRepository repository, IFileManager fileManager)
    {
        _repository = repository;
        _fileManager = fileManager;
    }
    
    public IActionResult Index(string category)
    {
        var posts = string.IsNullOrEmpty(category) ? _repository.GetAllPosts() : _repository.GetAllPosts(category);
        return View(posts);
    }
    
    public IActionResult Post(int id)
    {
        var post = _repository.GetPost(id);
        return View(post);
    }

    [HttpGet("/Image/{image}")]
    public IActionResult Image(string image)
    {
        var mime = image.Substring(image.LastIndexOf('.') + 1);
        return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mime}");
    }
}