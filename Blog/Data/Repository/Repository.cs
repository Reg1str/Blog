using Blog.Models;
using Blog.Models.Comments;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository;

public class Repository : IRepository
{
    private AppDbContext _ctx;

    public Repository(AppDbContext ctx)
    {
        _ctx = ctx;
    }
    
    public Post? GetPost(int id)
    {
        return _ctx.Posts
            .Include(p => p.MainComments)
                .ThenInclude(mc => mc.SubComments)
            .FirstOrDefault(p => p.Id == id);
    }

    public List<Post?> GetAllPosts()
    {
        return _ctx.Posts.ToList();
    }
    
    public List<Post?> GetAllPosts(string category)
    {
        Func<Post, bool> inCategory = (post) => post.Category.ToLower().Equals(category.ToLower());
        
        return _ctx.Posts.Where(post => post.Category.Equals(category)).ToList();
    }

    public void AddPost(Post? post)
    {
        _ctx.Posts.Add(post);
    }

    public void UpdatePost(Post? post)
    {
        _ctx.Posts.Update(post);
    }

    public void RemovePost(int id)
    {
        _ctx.Posts.Remove(GetPost(id));
    }

    public void AddSubComment(SubComment comment)
    {
        _ctx.SubComments.Add(comment);
    }

    public async Task<bool> SaveChangesAsync()
    {
        if (await _ctx.SaveChangesAsync() > 0)
        {
            return true;
        }

        return false;
    }
}