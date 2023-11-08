using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Database;
using UserService.Models;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserDbContext _userDbContext;

        public UserController(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var users = await _userDbContext.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] User userRequest)
        {
            await _userDbContext.Users.AddAsync(userRequest);
            await _userDbContext.SaveChangesAsync();
            return Ok(userRequest);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetStudent([FromRoute] int id)
        {
            var users = await _userDbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] int id, [FromBody] User updateRequest)
        {
            var users = await _userDbContext.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            users.UserId = updateRequest.UserId;
            users.UserName = updateRequest.UserName;
            users.Address = updateRequest.Address;

            await _userDbContext.SaveChangesAsync();

            return Ok(users);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            var users = await _userDbContext.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            _userDbContext.Users.Remove(users);
            await _userDbContext.SaveChangesAsync();
            return Ok(users);
        }
    }
}
