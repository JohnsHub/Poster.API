namespace Poster.API.Models
{
    public class Like
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public DateTime LikedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Post Post { get; set; }
        public AppUser User { get; set; }
    }

}
