using Blog.Helpers;
using Blog.Models;
using Blog.Models.Comments;
using Blog.ViewModels;
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

    public IndexViewModel GetAllPosts(
        int pageNumber,
        string category,
        string search)
    {
        Func<Post, bool> inCategory = (post) => post.Category.ToLower().Equals(category.ToLower());
        const int pageSize = 5;
        var skipAmount = pageSize * (pageNumber - 1);
        
        var query = _ctx.Posts.AsNoTracking().AsQueryable();

        if (!String.IsNullOrEmpty(category))
            query.Where(x => inCategory(x));

        if (!string.IsNullOrEmpty(search))
            query = query.Where(x => EF.Functions.Like(x.Title,$"%{search}%")
                                                                || EF.Functions.Like(x.Body,$"%{search}%")
                                                                || EF.Functions.Like(x.Description,$"%{search}%")); 
        


        var postsCount = query.Count();
        var pageCount = (int)Math.Ceiling((double)postsCount / pageSize);

        return new IndexViewModel
        {
            PageNumber = pageNumber,
            PageCount = pageCount,
            NextPage = postsCount > skipAmount + pageSize,
            Pages = PageHelper.PageNumbers(pageNumber, pageCount).ToList(),
            Category = category,
            Search = search,
            Posts = query
                .Skip(skipAmount)
                .Take(pageSize)
                .ToList()

        };
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