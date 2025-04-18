using System.ComponentModel.DataAnnotations;

namespace Poster.API.Models.DTOs
{
    public class UpdatePostDto
    {
        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
