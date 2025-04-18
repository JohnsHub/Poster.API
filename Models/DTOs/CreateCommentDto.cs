using System.ComponentModel.DataAnnotations;

namespace Poster.API.Models.DTOs
{
    public class CreateCommentDto
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
