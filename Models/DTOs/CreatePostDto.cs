using System.ComponentModel.DataAnnotations;

namespace Poster.API.Models.DTOs
{
    public class CreatePostDto
    {
        [Required]
        public string Content { get; set; }
    }
}
