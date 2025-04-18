using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Poster.API.Data;
using Poster.API.Models;
using Poster.API.Models.DTOs;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata.Ecma335;
using Microsoft.CodeAnalysis.CSharp;

namespace Poster.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CommentsController(AppDbContext context)
            => _context = context;

        // GET: api/comments?postId=123
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments([FromQuery] int postId)
        {
            var dtos = await _context.Comments
                .Where(c => c.PostId == postId)
                .OrderBy(c => c.CommentedAt)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    PostId = c.PostId,
                    Content = c.Content,
                    CommentedAt = c.CommentedAt,
                    UserName = c.User.UserName
                })
                .ToListAsync();

            return Ok(dtos);
        }

        // POST: api/comments
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CommentDto>> CreateComment([FromBody] CreateCommentDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var comment = new Comment
            {
                PostId = dto.PostId,
                Content = dto.Content,
                UserId = userId,
                CommentedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Map to DTO for the response
            var resultDto = new CommentDto
            {
                Id = comment.Id,
                PostId = comment.PostId,
                Content = comment.Content,
                CommentedAt = comment.CommentedAt,
                UserName = User.Identity!.Name!  // your JWT sub claim
            };

            return CreatedAtAction(
                nameof(GetComments),
                new { postId = resultDto.PostId },
                resultDto
            );
        }

        // DELETE: api/Comments/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (comment.UserId != userId)
                return Forbid();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Comments/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }
            _context.Entry(comment).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
