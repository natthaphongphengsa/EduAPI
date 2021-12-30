using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduAPI.Data;
using EduAPI.Data.Models;
using EduAPI.Dtos;

namespace EduAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Get all user from database
        /// </summary>
        /// <returns>return a list of all user</returns>
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            List<UserDto> userList = new List<UserDto>();
            foreach (var user in await _context.Users.ToListAsync())
            {
                userList.Add(new UserDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                });
            }
            return userList;
        }
        /// <summary>
        /// Find user by Id 
        /// </summary>
        /// <param name="id">User identity</param>
        /// <returns>Return User information if exist else Return NotFound</returns>
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            UserDto userDto = new UserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
            if (user == null)
            {
                return NotFound();
            }

            return userDto;
        }
        /// <summary>
        /// Update user by Id
        /// </summary>
        /// <param name="id">User Identity</param>
        /// <param name="userDtoModel">User object</param>
        /// <returns>Return Nothing if User exist else return BadRequest if user did not exist</returns>
        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDto userDtoModel)
        {
            //var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            //if (user == null)
            //{
            //    return BadRequest();
            //}

            _context.Entry(
                new User
                {
                    Id = id,
                    FirstName = userDtoModel.FirstName,
                    LastName = userDtoModel.LastName,
                    Email = userDtoModel.Email
                }).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        /// <summary>
        /// create user
        /// </summary>
        /// <param name="userDtoModel">User object</param>
        /// <returns>Return user that's has been create or return BadRequest if something is wrong
        /// Else return Not Acceptable</returns>
        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUser(UserDto userDtoModel)
        {
            User userModel = new User() { FirstName = userDtoModel.FirstName, LastName = userDtoModel.LastName, Email = userDtoModel.Email };
            if (!_context.Users.Any(u => u.Email == userModel.Email))
            {
                try
                {
                    _context.Users.Add(userModel);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetUser", new { id = userModel.Id }, userModel);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                return StatusCode(406, "User already exist!");
            }

        }
        /// <summary>
        /// find User by id and delete user if exist 
        /// else return NotFound
        /// </summary>
        /// <param name="id">User identity</param>
        /// <returns>Return nothing</returns>
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
