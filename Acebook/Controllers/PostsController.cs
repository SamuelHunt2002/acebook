using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using acebook.Models;
using acebook.ActionFilters;
using Microsoft.EntityFrameworkCore;


namespace acebook.Controllers;

[ServiceFilter(typeof(AuthenticationFilter))]
public class PostsController : Controller
{
    private readonly ILogger<PostsController> _logger;

    public PostsController(ILogger<PostsController> logger)
    {
        _logger = logger;
    }

    [Route("/posts")]
    [HttpGet]
    public IActionResult Index()
    {
        AcebookDbContext dbContext = new AcebookDbContext();
        var posts = dbContext.Posts
          .Include(p => p.User)  // Include user data for each post
          .ToList();
        posts.OrderBy(p => p.Id);
        ViewBag.Posts = posts;
        return View();
    }

    [Route("/posts")]
    [HttpPost]
    public IActionResult Create(Post post)
    {
        if (string.IsNullOrWhiteSpace(post.Content))
        {
            return RedirectToAction("Index");
        }
        else
        {
            AcebookDbContext dbContext = new AcebookDbContext();
            int currentUserId = HttpContext.Session.GetInt32("user_id").Value;
            post.UserId = currentUserId;
            dbContext.Posts.Add(post);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
      Route("/posts/like")]
     [HttpPost]
     public IActionResult AddLikes(int postId){
         AcebookDbContext dbContext = new AcebookDbContext();
         var post = dbContext.Posts.First(p => p.Id == postId);
         post.Likes += 1;
         dbContext.SaveChanges();
         return RedirectToAction("Index");
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
}

