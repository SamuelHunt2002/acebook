using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using acebook.Models;
using Microsoft.AspNetCore.Authentication;


namespace acebook.Controllers;

public class SessionsController : Controller
{
    private readonly ILogger<SessionsController> _logger;

    public SessionsController(ILogger<SessionsController> logger)
    {
        _logger = logger;
    }

    [Route("/signin")]
    [HttpGet]
    public IActionResult New()
    {
        return View();
    }

    [Route("/signin")]
    [HttpPost]
    public RedirectResult CreateSession(string email, string password) {
      AcebookDbContext dbContext = new AcebookDbContext();
  if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) {
    HttpContext.Session.SetString("login_error", "No username and/or password submitted!");
        return new RedirectResult("/signin");
  }
      User? user = dbContext.User.Where(user => user.Email.ToLower() == email.ToLower()).FirstOrDefault();
      if(user != null && user.Password == password)
      {
        HttpContext.Session.SetString("login_error", "");
        HttpContext.Session.SetInt32("user_id", user.Id);
        SessionHelper.SetName(HttpContext, user.Name);
        return new RedirectResult("/posts");
      } 
      else
      {
        HttpContext.Session.SetString("login_error", "Incorrect email and/or password!");
        return new RedirectResult("/signin");
      }
    }

  [Route ("/signout")]
  [HttpGet]
  
    public IActionResult Logout()
{
    HttpContext.SignOutAsync();
    HttpContext.Session.Clear();
    return RedirectToAction("Index", "Home");
}


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
