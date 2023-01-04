using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using acebook.Models;
using acebook.ActionFilters;


namespace acebook.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [Route("/")]
    public IActionResult Index()
    {
        
       bool isLoggedIn = SessionHelper.IsUserLoggedIn(HttpContext);
   ViewBag.isLoggedIn = isLoggedIn;
        if (isLoggedIn)
        {
            // User is logged in
            ViewBag.Message = "Logged in";
            ViewBag.User_Id = HttpContext.Session.GetInt32("user_id").Value;
        }
        else
        {
            // User is not logged in
            ViewBag.Message = "Not logged in";
            ViewBag.User_Id = "";
        }
        return View();
    }
    [Route ("/privacy")]
    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }

     [Route ("/policy")]
    [HttpGet]
    public IActionResult Policy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
