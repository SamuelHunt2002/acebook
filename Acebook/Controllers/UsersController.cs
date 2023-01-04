using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using acebook.Models;
using Microsoft.AspNetCore.Http;

namespace acebook.Controllers
{
    public class UsersController : Controller
{
    private readonly ILogger<UsersController> _logger;
    private readonly AcebookDbContext _context;

 public UsersController(AcebookDbContext context)
    {
        _context = context;
    }

    public UsersController(ILogger<UsersController> logger, AcebookDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    private int GetCurrentUserId()
    {
        // Retrieve the user ID from the session
        var userIdString = HttpContext.Session.GetString("UserId");

        // Parse the user ID from the session and return it
        return int.Parse(userIdString);
    }

    [Route("/signup")]
    [HttpGet]
    public IActionResult New()
    {
        return View();
    }

    [Route("/users")]
    [HttpPost]
    public RedirectResult Create(User user) 
    {
      _context.Users.Add(user);
      _context.SaveChanges();
      return new RedirectResult("/signin");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
}
