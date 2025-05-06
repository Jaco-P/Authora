using Authora.Application.Interfaces;
using Authora.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Authora.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Permission>>> GetAll()
        {
            var permissions = await _permissionService.GetAllAsync();
            return Ok(permissions);
        }

        [HttpGet("by-group/{groupId}")]
        public async Task<ActionResult<IEnumerable<Permission>>> GetByGroup(Guid groupId)
        {
            var permissions = await _permissionService.GetByGroupIdAsync(groupId);
            return Ok(permissions);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Permission permission)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _permissionService.AddAsync(permission);
            return CreatedAtAction(nameof(GetByGroup), new { groupId = permission.GroupId }, permission);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _permissionService.DeleteAsync(id);
            return NoContent();
        }
    }
}
