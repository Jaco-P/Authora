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
            try
            {
                var groups = await _groupService.GetAllAsync();
                return Ok(groups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving groups: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetById(Guid id)
        {
            try
            {
                var group = await _groupService.GetByIdAsync(id);
                if (group == null)
                    return NotFound();

                return Ok(group);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the group: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Group group)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _groupService.AddAsync(group);
                return CreatedAtAction(nameof(GetById), new { id = group.Id }, group);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the group: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _groupService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the group: {ex.Message}");
            }
        }
    }
}