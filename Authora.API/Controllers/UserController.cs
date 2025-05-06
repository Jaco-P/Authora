using Authora.Application.Interfaces;
using Authora.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Authora.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            return Ok(await _userService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] User user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            user.Id = Guid.NewGuid();
            await _userService.AddAsync(user);
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] User updated)
        {
            if (id != updated.Id) return BadRequest("ID mismatch");
            await _userService.UpdateAsync(updated);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetUserCount()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users.Count);
        }

        [HttpGet("count-per-group")]
        public async Task<ActionResult<Dictionary<string, int>>> GetUserCountPerGroup()
        {
            var users = await _userService.GetAllAsync();
            var result = users
                .SelectMany(u => u.UserGroups)
                .GroupBy(ug => ug.Group.Name)
                .ToDictionary(g => g.Key, g => g.Count());

            return Ok(result);
        }
    }
}
