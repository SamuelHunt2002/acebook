namespace acebook.Models;
using System.ComponentModel.DataAnnotations;

public class FriendRequest
{
  [Key]
  public int Id {get; set;}

public User? User {get; set;}

public int SenderId {get; set;}

public int RecipientId {get; set;}

public bool Accepted {get; set;}

public User Recipient {
    get {
        using (var dbContext = new AcebookDbContext()) {
            return dbContext.Users.FirstOrDefault(user => user.Id == this.RecipientId);
        }
    }
}


}