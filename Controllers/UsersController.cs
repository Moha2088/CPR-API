using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CVR_API.Models.Dtos;
using CVR_API.Models;
using CVR_API.Repository;
using Microsoft.AspNetCore.Cors;
using CVR_API.Services;

namespace CVR_API.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _service.GetUsers();
            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]   
        public async Task<ActionResult<UserDTO>> GetUser(Guid id)
        {
            var user = await _service.GetUser(id);
            return Ok(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
           await _service.PutUser(id, user);
           return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var userToPost = await _service.PostUser(user);
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _service.DeleteUser(id);
            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return _service.UserExists(id);
        }
    }
}
