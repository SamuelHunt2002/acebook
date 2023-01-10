using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using acebook.Models;

namespace acebook.Controllers
{
    public class FriendRequestsController : Controller
    {
        [Route("/friends/new")]
        [HttpGet]
        public IActionResult New()

        {
            AcebookDbContext dbContext = new AcebookDbContext();
            var requests = dbContext.FriendRequests
            .Include(fr => fr.User)
           .ToList();
            return View(requests);
        }



        [Route("/friends")]
        [HttpPost]
        public IActionResult SendFriendRequest(string recipientEmail, string message)
        {
            if (string.IsNullOrEmpty(recipientEmail) || string.IsNullOrWhiteSpace(recipientEmail))
            {
                ModelState.AddModelError("RecipientEmail", "Recipient email is required");
                return View("New");
            }

            AcebookDbContext dbContext = new AcebookDbContext();
            var recipient = dbContext.Users.FirstOrDefault(user => user.Email.ToLower() == recipientEmail.ToLower());
            if (recipient == null)
            {
                ModelState.AddModelError("RecipientEmail", "Recipient not found");
                return View("New");
            }

            var senderId = HttpContext.Session.GetInt32("user_id").Value;
            var friendRequest = new FriendRequest
            {
                SenderId = senderId,
                RecipientId = recipient.Id,
                Accepted = false
            };
            dbContext.FriendRequests.Add(friendRequest);
            dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

    }
}
