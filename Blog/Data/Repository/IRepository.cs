using Blog.Models;
using Blog.Models.Comments;
using Blog.ViewModels;

namespace Blog.Data.Repository;

public interface IRepository
{
    Post? GetPost(int id);
    List<Post?> GetAllPosts();
    IndexViewModel GetAllPosts(int pageNumber, string category, string search);
    void AddPost(Post? post);
    void UpdatePost(Post? post);
    void RemovePost(int id);
    void AddSubComment(SubComment comment);
    Task<bool> SaveChangesAsync();
}