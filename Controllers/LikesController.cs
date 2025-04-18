using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Poster.API.Data;
using Poster.API.Models;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace Poster.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LikesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Like?postId=1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Like>>> GetLikes([FromQuery] int postId)
        {
            var likes = await _context.Likes
                .Where(l => l.PostId == postId)
                .ToListAsync();
            return Ok(likes);
        }

        // POST: api/Like
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Like>> CreateLike([FromBody] Like like)
        {
            like .UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Timestamp it
            like.LikedAt = DateTime.UtcNow;
            _context.Likes.Add(like);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLikes), new { id = like.Id }, like);
        }

        // DELETE: api/Like/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLike(int id)
        {
            var like = await _context.Likes.FindAsync(id); // First assign like to id
            if ( like == null) // Check if like is null
            {
                return NotFound(); // So this does not crash
            }

            _context.Likes.Remove(like); // Remove the like
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/Like/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLike(int id, [FromBody] Like like)
        {
            if (id != like.Id)
            {
                return BadRequest();
            }
            _context.Entry(like).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LikeExists(like.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool LikeExists(int id)
        {
            return _context.Likes.Any(e => e.Id == id);
        }

    }
}
