using Blog.Data.FileManager;
using Blog.Data.Repository;
using Blog.Models.Comments;
using Blog.ViewModels;
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

    public IActionResult Index(int pageNumber, string category)
    {
        if (pageNumber < 1)
            return RedirectToAction("Index", new { pageNumber = 1, category});
        
        var indexViewModel = _repository.GetAllPosts(pageNumber, category);
        
        return View(indexViewModel);
    }

    public IActionResult Post(int id) => 
            View(_repository.GetPost(id));
    
    [HttpGet("/Image/{image}")]
    [ResponseCache(CacheProfileName = "Monthly")]
        public IActionResult Image(string image) => 
            new FileStreamResult(
                _fileManager.ImageStream(image), 
                $"image/{image.Substring(image.LastIndexOf('.') + 1)}");

        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel commentViewModel)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Post", new { id = commentViewModel.PostId});

            var post = _repository.GetPost(commentViewModel.PostId);
            if (commentViewModel.MainCommentId == 0)
            {
                post.MainComments = post.MainComments ?? new List<MainComment>();
                
                post.MainComments.Add(new MainComment
                {
                    Message = commentViewModel.Message,
                    Created = DateTime.Now,
                });
                
                _repository.UpdatePost(post);
            }
            else
            {
                var comment = new SubComment
                {
                    MainCommentId = commentViewModel.MainCommentId,
                    Message = commentViewModel.Message,
                    Created = DateTime.Now,
                };
                
                _repository.AddSubComment(comment);
            }

            await _repository.SaveChangesAsync();
            
            return RedirectToAction("Post", new { id = commentViewModel.PostId});
        }
        
    // public IActionResult Index(string category)
    // {
    //     var posts = string.IsNullOrEmpty(category) ? _repository.GetAllPosts() : _repository.GetAllPosts(category);
    //     return View(posts);
    // }

    // public IActionResult Post(int id)
    // {
    //     var post = _repository.GetPost(id);
    //     return View(post);
    // }

    // [HttpGet("/Image/{image}")]
    // public IActionResult Image(string image)
    // {
    //     var mime = image.Substring(image.LastIndexOf('.') + 1);
    //     return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mime}");
    // }
}