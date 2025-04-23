using System.ComponentModel.DataAnnotations;

namespace Poster.API.Models.DTOs
{
    public class UpdateProfileDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string UserName { get; set; }
        public string ?Bio { get; set; }
        public string ?Location { get; set; }
    }
}
