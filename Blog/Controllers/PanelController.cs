using Blog.Data.FileManager;
using Blog.Data.Repository;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Authorize(Roles = "Admin")]
public class PanelController : Controller
{
    private IRepository _repository;
    private IFileManager _fileManager;

    public PanelController(IRepository repository, IFileManager fileManager)
    {
        _repository = repository;
        _fileManager = fileManager;
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
            return View(new PostViewModel());
        }
        else
        {
            var post = _repository.GetPost((int)id);
            return View(new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                CurrentImage = post.Image,
                Description = post.Description,
                Tags = post.Tags,
                Category = post.Category
            });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(PostViewModel postViewModel)
    {
        var post = new Post()
        {
            Id = postViewModel.Id,
            Title = postViewModel.Title,
            Body = postViewModel.Body,
            Description = postViewModel.Description,
            Tags = postViewModel.Tags,
            Category = postViewModel.Category
        };

        if (postViewModel.Image == null)
            post.Image = postViewModel.CurrentImage;
        else
            post.Image = await _fileManager.SaveImage(postViewModel.Image);
        
            
        if (post.Id > 0)
            _repository.UpdatePost(post);
        else
            _repository.AddPost(post);
        
        if (await _repository.SaveChangesAsync())
            return RedirectToAction("Index");
        else
            return View(postViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Remove(int id)
    {
        _repository.RemovePost(id);
        await _repository.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}