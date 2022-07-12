namespace Blog.Models;

public class Post
{
    public string Title { get; set; } = "";
    public string Body { get; set; } = "";
    
    public DateTime Created { get; set; } = DateTime.Now;
}