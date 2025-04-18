namespace Poster.API.Models
{
    public class Like
    {
        public int Id { get; set; }
        public string UserId { get; set; }  // or reference a User model
        public int PostId { get; set; }
        public DateTime LikedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Post Post { get; set; }
        public User User { get; set; }
    }

}
