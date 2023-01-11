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



        [Route("/friends/request")]
        [HttpPost]
        public IActionResult SendFriendRequest(string recipientEmail)
        {
            if (string.IsNullOrEmpty(recipientEmail) || string.IsNullOrWhiteSpace(recipientEmail))
            {
                ModelState.AddModelError("RecipientEmail", "Recipient email is required");
               return RedirectToAction("New", "FriendRequests");
            }

            AcebookDbContext dbContext = new AcebookDbContext();
            var recipient = dbContext.Users.FirstOrDefault(user => user.Email.ToLower() == recipientEmail.ToLower());
            if (recipient == null)
            {
                ModelState.AddModelError("RecipientEmail", "Recipient not found");
                return RedirectToAction("New", "FriendRequests");
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

            return RedirectToAction("New", "FriendRequests");
        }


        [Route("/friends/accept")]
        [HttpPost]
        public IActionResult AcceptFriendRequest(int requestId)
        {
            AcebookDbContext dbContext = new AcebookDbContext();
            FriendRequest friendRequest = dbContext.FriendRequests.FirstOrDefault(f => f.Id == requestId);
            friendRequest.Accepted = true;
            dbContext.SaveChanges();
            return RedirectToAction("New", "FriendRequests");
        }

        [Route("/friends/delete")]
        [HttpPost]
        public IActionResult DeleteFriendRequest(int requestId)
        {
            AcebookDbContext dbContext = new AcebookDbContext();
            FriendRequest friendRequest = dbContext.FriendRequests.FirstOrDefault(f => f.Id == requestId);
            dbContext.FriendRequests.Remove(friendRequest);
            dbContext.SaveChanges();
            return RedirectToAction("New", "FriendRequests");
        }
    }
}
