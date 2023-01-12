using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using acebook.Models;
using acebook.ActionFilters;
using Microsoft.EntityFrameworkCore;


namespace acebook.Controllers;


public class MessagesController : Controller
{  private readonly ILogger<MessagesController> _logger;

    public MessagesController(ILogger<MessagesController> logger)
    {
        _logger = logger;
    }

    [Route("/messages")]
    [HttpGet]
    public IActionResult Index() {
        AcebookDbContext acebookDbContext = new AcebookDbContext();
        int userId = HttpContext.Session.GetInt32("user_id").Value;
        var messages = acebookDbContext.Messages
    .Include(m => m.Sender)
    .Include(m => m.Recipient)
    .Where(m => m.SenderId == userId || m.RecipientId == userId)
    .ToList();
        return View(messages);
    }


}