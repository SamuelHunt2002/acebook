namespace acebook.Models;
using System.ComponentModel.DataAnnotations;

public class Comment
{
  [Key]
  public int Id {get; set;}
  public string? Content {get; set;}
  public int UserId {get; set;}
  public User? User {get; set;}
  public int PostID {get; set;}
  public Post? Post {get; set;}

}
