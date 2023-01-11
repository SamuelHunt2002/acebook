using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using acebook.Models;
using acebook.ActionFilters;

namespace acebook.Models
{


    public class SessionHelper
    {

        public static void SetName(HttpContext context, string name)
        {
            context.Session.SetString("Name", name);
        }

        public static string GetName(HttpContext context)
        {
            return context.Session.GetString("Name");
        }


        public static bool IsUserLoggedIn(HttpContext httpContext)
        {
            try
            {
                int userId = httpContext.Session.GetInt32("user_id").Value;
                return true;
            }
            catch (InvalidOperationException)
            {
                // Session value is null
                return false;
            }
        }

            public bool IsFriendWith(int userId, int otherUserId)
    {
        AcebookDbContext dbContext = new AcebookDbContext();
        var friendRelation = dbContext.FriendRequests
            .Where(fr => (fr.SenderId == userId && fr.RecipientId == otherUserId) || (fr.SenderId == otherUserId && fr.RecipientId == userId))
            .Where(fr => fr.Accepted)
            .FirstOrDefault();

        return friendRelation != null;
    }
}

    }
