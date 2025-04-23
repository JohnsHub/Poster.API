using System;
using System.Collections.Generic;

namespace Poster.API.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string DisplayName { get; set; } = string.Empty;

        // Tie to User
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Retweet> Retweets { get; set; } = new List<Retweet>();
    }
}
