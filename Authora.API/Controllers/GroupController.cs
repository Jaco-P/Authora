using Authora.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Authora.Domain.Entities;

namespace Authora.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetAll()
        {
            var groups = await _groupService.GetAllAsync();
            return Ok(groups);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetById(Guid id)
        {
            var group = await _groupService.GetByIdAsync(id);
            if (group == null)
                return NotFound();

            return Ok(group);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Group group)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _groupService.AddAsync(group);
            return CreatedAtAction(nameof(GetById), new { id = group.Id }, group);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _groupService.DeleteAsync(id);
            return NoContent();
        }
    }

}
