namespace Poster.API.Models.DTOs
{
    public class UserProfileDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Bio { get; set; }
        public string? Location { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreateAt { get; set; }
        public List<PostDto> Posts { get; set; } = new();
    }
}
