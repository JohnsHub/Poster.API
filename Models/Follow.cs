namespace Poster.API.Models
{
    public class Follow
    {
        public int Id { get; set; }
        public string FollowerId { get; set; } = string.Empty;
        public AppUser Follower { get; set; } = null!;
        public string FolloweeId { get; set; } = string.Empty;
        public AppUser Followee { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
