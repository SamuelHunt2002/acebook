namespace acebook.Models;
using System.ComponentModel.DataAnnotations;

public class FriendRequestViewModel
{
    [Required]
    [EmailAddress]
    public string RecipientEmail { get; set; }
    public string Message { get; set; }
}
