using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Poster.API.Data;
using Poster.API.Models;

namespace Poster.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public PostsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            return await _context.Posts.OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost(Post post)
        {
            post.CreatedAt = DateTime.UtcNow;
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPosts), new { id = post.Id }, post);
        }
    }
}
