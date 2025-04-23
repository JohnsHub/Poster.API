// Controllers/FollowsController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Poster.API.Data;
using Poster.API.Models;
using Poster.API.Models.DTOs;

[ApiController]
[Route("api/[controller]")]
public class FollowsController : ControllerBase
{
    private readonly AppDbContext _context;
    public FollowsController(AppDbContext context) => _context = context;

    // GET /api/follows?followerId=...&followeeId=...
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FollowDto>>> GetFollows(
        [FromQuery] string? followerId,
        [FromQuery] string? followeeId)
    {
        var q = _context.Follows
            .Include(f => f.Follower)
            .Include(f => f.Followee)
            .AsQueryable();

        if (followerId != null) q = q.Where(f => f.FollowerId == followerId);
        if (followeeId != null) q = q.Where(f => f.FolloweeId == followeeId);

        var list = await q
            .Select(f => new FollowDto
            {
                Id = f.Id,
                FollowerUsername = f.Follower.UserName,
                FolloweeUsername = f.Followee.UserName,
                CreatedAt = f.CreatedAt
            })
            .ToListAsync();

        return Ok(list);
    }

    // POST /api/follows
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<FollowDto>> CreateFollow([FromBody] CreateFollowDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        if (userId == dto.FolloweeId)
            return BadRequest("Cannot follow yourself.");

        var already = await _context.Follows
            .AnyAsync(f => f.FollowerId == userId && f.FolloweeId == dto.FolloweeId);
        if (already) return BadRequest("Already following.");

        var follow = new Follow
        {
            FollowerId = userId,
            FolloweeId = dto.FolloweeId,
            CreatedAt = DateTime.UtcNow
        };
        _context.Follows.Add(follow);
        await _context.SaveChangesAsync();

        // reload usernames
        var follower = await _context.Users.FindAsync(userId)!;
        var followee = await _context.Users.FindAsync(dto.FolloweeId)!;

        var result = new FollowDto
        {
            Id = follow.Id,
            FollowerUsername = follower.UserName,
            FolloweeUsername = followee.UserName,
            CreatedAt = follow.CreatedAt
        };
        return CreatedAtAction(nameof(GetFollows),
            new { followerId = userId }, result);
    }

    // DELETE /api/follows/5
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFollow(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var follow = await _context.Follows.FindAsync(id);
        if (follow == null) return NotFound();
        if (follow.FollowerId != userId) return Forbid();

        _context.Follows.Remove(follow);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
