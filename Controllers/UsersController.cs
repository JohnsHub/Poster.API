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
            _context = context.Where(c.id => 
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromQuery] int id)
        {
            var users = await _context.Users;
            
        }
    }

}
