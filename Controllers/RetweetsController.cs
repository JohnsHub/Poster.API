using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Poster.API.Data;
using Poster.API.Models;

namespace Poster.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RetweetsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RetweetsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Retweets?postId=1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Retweet>>> GetRetweets([FromQuery] int postId)
        {
            var retweets = await _context.Retweets
                .Where(r => r.PostId == postId)
                .ToListAsync();
            return Ok(retweets);
        }

        // POST: api/Retweets
        [HttpPost]
        public async Task<ActionResult<Retweet>> CreateRetweet([FromBody] Retweet retweet)
        {
            retweet.RetweetedAt = DateTime.UtcNow;
            _context.Retweets.Add(retweet);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRetweets), new { postId = retweet.PostId }, retweet);
        }

        // DELETE: api/Retweets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRetweet(int id)
        {
            var retweet = await _context.Retweets.FindAsync(id);
            if (retweet == null)
            {
                return NotFound();
            }
            _context.Retweets.Remove(retweet);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/Retweets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRetweet(int id, [FromBody] Retweet retweet)
        {
            if (id != retweet.Id)
            {
                return BadRequest();
            }
            _context.Entry(retweet).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RetweetExists(id))
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

        private bool RetweetExists(int id)
        {
            return _context.Retweets.Any(e => e.Id == id);
        }
    }
}
