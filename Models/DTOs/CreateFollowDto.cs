using System.ComponentModel.DataAnnotations;

public class CreateFollowDto
{
    [Required]
    public string FolloweeId { get; set; } = string.Empty;
}

// Models/DTOs/FollowDto.cs
public class FollowDto
{
    public int Id { get; set; }
    public string FollowerUsername { get; set; } = string.Empty;
    public string FolloweeUsername { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
