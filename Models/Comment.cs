namespace Poster.API.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public DateTime CommentedAt { get; set; } = DateTime.UtcNow;

        

        // Navigation properties
        public Post Post { get; set; }
        public User User { get; set; }
    }

}
