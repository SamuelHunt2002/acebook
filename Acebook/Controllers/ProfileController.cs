using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using acebook.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;


namespace acebook.Controllers;


public class ProfileController : Controller
{
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(ILogger<ProfileController> logger)
    {
        _logger = logger;
    }

    [Route("/profile")]
    [HttpGet]
    public IActionResult Index()
    {
        // Get the user's ID from the session
        if (HttpContext.Session.GetInt32("user_id") == null)
        {
            // Redirect the user to the login page if they are not logged in
            return RedirectToAction("Index", "Home");
        }
        int userId = HttpContext.Session.GetInt32("user_id").Value;
        // Retrieve the user's data from the database
        AcebookDbContext dbContext = new AcebookDbContext();
        var user = dbContext.Users
            .Include(u => u.Posts)  // Include related posts
            .Where(u => u.Id == userId)
            .First();

        // Pass the user data to the view
        ViewBag.User = user;


        return View(user);
    }

    [Route("/profile/{userId}")]
    [HttpGet]
    public IActionResult Show(int userId)
    {
        // Retrieve the user's data from the database
        AcebookDbContext dbContext = new AcebookDbContext();
        var user = dbContext.Users
            .Include(u => u.Posts)  // Include related posts
            .Where(u => u.Id == userId)
            .First();

        // Pass the user data to the view
        ViewBag.User = user;

        return View(user);
    }

    [Route("/searchuser")]
    [HttpGet]
    public IActionResult SearchResult(int userId)
{
    AcebookDbContext dbContext = new AcebookDbContext();
    var loggedInId = HttpContext.Session.GetInt32("user_id").Value;
    User user = dbContext.Users.Where(u => u.Id == userId).Include(u => u.Posts).First();
    SessionHelper sessionHelper = new SessionHelper();
    ViewBag.SessionId = loggedInId;
    ViewBag.IsFriendWith = sessionHelper.IsFriendWith(loggedInId, userId);
    return View(user);
}

    [Route("/search")]
    [HttpGet]
    public IActionResult Search()
{
    return View();
}

}
