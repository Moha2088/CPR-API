using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CVR_API.Models.Dtos;
using CVR_API.Models;
using Microsoft.AspNetCore.Cors;
using CVR_API.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;

namespace CVR_API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [EnableRateLimiting("fixed")]
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
        ///<summary>
        /// Gets a list of all users converted to a DTO
        ///</summary>
        ///<response code="200">Returns OK with a list of all users converted to a DTO</response>
        ///<response code="404">Returns NotFound if the list is empty</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        ///<summary>
        /// Gets a user based on a specific id
        ///</summary>
        ///<response code="200">Returns OK and a DTO version of the object</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDTO>> GetUser(Guid id)
        {
            var user = await _service.GetUser(id);
            return Ok(user);
        }

        // PUT: api/Users/5
        ///<summary>
        /// Updates a user based on a specific id
        ///</summary>
        ///<response code="204">Returns nothing</response>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            await _service.PutUser(id, user);
            return NoContent();
        }

        // POST: api/Users
        ///<summary>
        /// Creates a user
        ///</summary>
        ///<remarks>
        /// Example request:
        /// 
        ///     POST /users
        ///     {
        ///         "name": "Test",
        ///         "cpr": "111111-1111"
        ///     }
        /// 
        ///</remarks>>
        ///<response code = "201">Returns a converted version of the newly created object </response>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<UserDTO>> PostUser(User user)
        {
            var userToPost = await _service.PostUser(user);
            return CreatedAtAction("GetUser", new { id = userToPost.Id }, userToPost);
        }

        // DELETE: api/Users/5
        ///<summary>
        /// Deletes a user based on a specific id
        ///</summary>
        ///<response code = "204">Returns nothing</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
