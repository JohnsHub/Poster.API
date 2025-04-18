using System;

namespace Poster.API.Models.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CommentedAt { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
