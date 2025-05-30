﻿using Microsoft.AspNetCore.Authorization;
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
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PostsController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts()
        {
            var dtos = await _context.Posts
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    UserName = p.User.UserName,
                    UserId = p.UserId,
                    DisplayName = p.DisplayName
                })
                .ToListAsync();

            return Ok(dtos);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PostDto>> CreatePost([FromBody] CreatePostDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var post = new Post
            {
                Content = dto.Content,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            var resultDto = new PostDto
            {
                Id = post.Id,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                UserName = User.Identity!.Name!
            };

            return CreatedAtAction(
                nameof(GetPosts),
                new { id = post.Id },
                resultDto
            );
        }

        // DELETE: api/posts/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Include(p => p.Retweets)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (post.UserId != userId)
                return Forbid();

            if (post.Comments.Any())
                _context.Comments.RemoveRange(post.Comments);

            if (post.Likes.Any())
                _context.Likes.RemoveRange(post.Likes);

            if (post.Retweets.Any())
                _context.Retweets.RemoveRange(post.Retweets);

            _context.Posts.Remove(post);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/posts/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] UpdatePostDto dto)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
                return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (post.UserId != userId)
                return Forbid();
            post.Content = dto.Content;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
    }
