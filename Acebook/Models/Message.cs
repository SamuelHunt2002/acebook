using System.ComponentModel.DataAnnotations;

namespace acebook.Models
{
    public class Message
    {
        [Key]
        public int Id {get; set;}
        public int SenderId {get; set;}
        public User Sender {get; set;}
        public int RecipientId {get; set;}
        public User Recipient {get; set;}
        public string Content {get; set;}
    }
}
