using Blog.Models;

namespace Blog.ViewModels;

public class IndexViewModel
{
    public int PageNumber { get; set; }
    public bool NextPage { get; set; }
    public string Category { get; set; }
    public IEnumerable<Post> Posts { get; set; }
}