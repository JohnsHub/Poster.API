using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Poster.API.Data;
using Poster.API.Models;

namespace Poster.API.Controllers {
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : Controllerbase 
    {
        private readonly AppDbContext _context;

        public UserController (AppDbContext context)
        {
            _context = context;

        }

        [HttpGet]
public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromQuery] int userId)
{
    var users = await _context.Users
        .Where(u => u.UserId == userId)
        .ToListAsync();
    return Ok(users);
}

[HttpPost]
public async Task<ActionResult<User>> CreateUser([FromBody] User user)
{
    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    // Returns a 201 Created response with a route to fetch the created user.
    // Ensure you have an action like GetUserById to match this route.
    return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
}

[HttpPut("{id}")]
public async Task<ActionResult> UpdateUser(int id, [FromBody] User user)
{
    if (id != user.UserId)
    {
        return BadRequest("The provided ID does not match the User's ID.");
    }

    _context.Entry(user).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!_context.Users.Any(u => u.UserId == id))
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
            
        }
    }

}
