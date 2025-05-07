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
            try
            {
                var permissions = await _permissionService.GetAllAsync();
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving permissions: {ex.Message}");
            }
        }

        [HttpGet("by-group/{groupId}")]
        public async Task<ActionResult<IEnumerable<Permission>>> GetByGroup(Guid groupId)
        {
            try
            {
                var permissions = await _permissionService.GetByGroupIdAsync(groupId);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving permissions for group {groupId}: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Permission permission)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _permissionService.AddAsync(permission);
                return CreatedAtAction(nameof(GetByGroup), new { groupId = permission.GroupId }, permission);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the permission: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _permissionService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the permission: {ex.Message}");
            }
        }
    }
}