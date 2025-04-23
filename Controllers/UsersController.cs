using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Poster.API.Data;
using Poster.API.Models;
using Poster.API.Models.DTOs;
using System.Security.Claims;

namespace Poster.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext ctx) => _context = ctx;

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUserById(string id)
        {
            var user = await _context.Users
                .Include(u => u.Posts)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            var dto = new UserProfileDto
            {
                Id = user.Id,
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Bio = user.Bio,
                Location = user.Location,
                CreateAt = user.CreatedAt,
                Posts = user.Posts
                            .OrderByDescending(p => p.CreatedAt)
                            .Select(p => new PostDto
                            {
                                Id = p.Id,
                                Content = p.Content,
                                CreatedAt = p.CreatedAt,
                                UserName = user.UserName,
                                DisplayName = user.DisplayName,
                                UserId = user.Id
                            })
                            .ToList()
            };
            return Ok(dto);
        }

        // PUT: api/users/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult>Updateprofile(string id, [FromBody] UpdateProfileDto dto)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != id) return Forbid();

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            // Apply changes
            user.DisplayName = dto.DisplayName;
            user.UserName = dto.UserName;
            user.Bio = dto.Bio ?? user.Bio;
            user.Location = dto.Location ?? user.Location;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/users/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
