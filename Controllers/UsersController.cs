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
            
        }
    }

}
