using Blog.Models;

namespace Blog.Data.Repository;

public interface IRepository
{
    Post? GetPost(int id);
    List<Post?> GetAllPosts();
    void AddPost(Post? post);
    void UpdatePost(Post? post);
    void RemovePost(int id);
    Task<bool> SaveChangesAsync();
}