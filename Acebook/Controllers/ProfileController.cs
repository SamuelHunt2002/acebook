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
        int userId = HttpContext.Session.GetInt32("user_id").Value;
        Console.WriteLine("USER ID IS HERE");
        Console.WriteLine(userId);
        // Retrieve the user's data from the database
        AcebookDbContext dbContext = new AcebookDbContext();
var user = dbContext.Users
    .Include(u => u.Posts)  // Include related posts
    .Where(u => u.Id == userId)
    .First();

        // Pass the user data to the view
        ViewBag.User = user;
        ViewBag.User = user;

        return View(user);
    }
}
