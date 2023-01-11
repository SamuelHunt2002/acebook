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
          .Include(p => p.User)
          .Include(p => p.Comments)
          .ToList();
        posts.Reverse();
        ViewBag.Posts = posts;
        return View();
    }

    [Route("/friendsposts")]
    [HttpGet]
    public IActionResult FriendsPosts()
    {
     AcebookDbContext dbContext = new AcebookDbContext();
    int userId = HttpContext.Session.GetInt32("user_id").Value;
    var friends = dbContext.FriendRequests.Where(fr => fr.SenderId == userId || fr.RecipientId == userId)
        .Where(fr => fr.Accepted == true)
        .Select(fr => fr.SenderId == userId ? fr.RecipientId : fr.SenderId)
        .ToList();
        Console.WriteLine("HERE");
    Console.WriteLine(friends.ToString());
    Console.WriteLine(friends.Count().ToString());
    var posts = dbContext.Posts
        .Include(p => p.User)
        .Include(p => p.Comments)
        .ThenInclude(p => p.User)
        .Where(p => friends.Contains(p.UserId))
        .ToList();

    posts.Reverse();
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

    [Route("/posts/{postId}/comments")]
    [HttpPost]
    public IActionResult CreateComment(int postId, Comment comment)
    {
        if (string.IsNullOrWhiteSpace(comment.Content))
        {
            return RedirectToAction("Index");
        }
        else
        {
            AcebookDbContext dbContext = new AcebookDbContext();
            int currentUserId = HttpContext.Session.GetInt32("user_id").Value;
            comment.UserId = currentUserId;
            comment.PostID = postId;
            dbContext.Comments.Add(comment);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }





    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
