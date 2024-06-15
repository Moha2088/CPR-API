using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CVR_API.Models.Dtos;
using CVR_API.Models;
using Microsoft.AspNetCore.Cors;
using CVR_API.Services;
using Asp.Versioning;

namespace CVR_API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [EnableCors("MyPolicy")]
    [ApiVersion("1.0")]
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

            if (!users.Any())
            {
                return NotFound("No users exist!");
            }

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
        public async Task<ActionResult<UserDTO>> PostUser(User user)
        {
            var userToPost = await _service.PostUser(user);
            return CreatedAtAction("GetUser", new { id = userToPost.Id }, userToPost);
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
