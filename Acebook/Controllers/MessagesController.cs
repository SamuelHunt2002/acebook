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

    [Route("/messages")]
    [HttpPost]
    public IActionResult Send(string input, string targetEmail) {
        AcebookDbContext acebookDbContext = new AcebookDbContext();
        Message message = new Message();
        var recipient = acebookDbContext.Users.FirstOrDefault(u => u.Email == targetEmail);
        if (recipient == null) {
            TempData["MessageEmailNotFound"] = "Recipient email is not found";
            return RedirectToAction("Index");
        }
        int recipientId = recipient.Id;
        SessionHelper sessionHelper = new SessionHelper();
        
        message.SenderId = HttpContext.Session.GetInt32("user_id").Value;
        if (sessionHelper.IsFriendWith(message.SenderId, recipientId)) {
            message.RecipientId = recipientId;
            message.Content = input; 
            Console.WriteLine("Here");
            Console.WriteLine(message.Content);
            acebookDbContext.Messages.Add(message);
            acebookDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        else {
           TempData["MessageEmailIsNotFriend"] = "Recipient not a friend yet!";

            return View("Index");
        }
    }


}