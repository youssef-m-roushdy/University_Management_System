using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University_Management_System.Application.Contracts;
using University_Management_System.Application.Dtos.AuthDtos;
using University_Management_System.Application.Dtos.RoleDtos;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public RolesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<RoleDto>>>> GetAllRoles()
        {
            var roles = await _serviceManager.RoleService.GetAllRolesAsync();
            return Ok(ApiResponse<IEnumerable<RoleDto>>.SuccessResponse(
                roles, 
                "Roles retrieved successfully"
            ));
        }

        /// <summary>
        /// Get role by ID
        /// </summary>
        [HttpGet("{roleId}")]
        public async Task<ActionResult<ApiResponse<RoleDto>>> GetRoleById(string roleId)
        {
            var role = await _serviceManager.RoleService.GetRoleByIdAsync(roleId);
            if (role == null)
                return NotFound(ApiResponse<RoleDto>.NotFoundResponse($"Role with ID '{roleId}' not found."));

            return Ok(ApiResponse<RoleDto>.SuccessResponse(role, "Role retrieved successfully"));
        }

        /// <summary>
        /// Get role by name
        /// </summary>
        [HttpGet("by-name/{roleName}")]
        public async Task<ActionResult<ApiResponse<RoleDto>>> GetRoleByName(string roleName)
        {
            var role = await _serviceManager.RoleService.GetRoleByNameAsync(roleName);
            if (role == null)
                return NotFound(ApiResponse<RoleDto>.NotFoundResponse($"Role '{roleName}' not found."));

            return Ok(ApiResponse<RoleDto>.SuccessResponse(role, "Role retrieved successfully"));
        }

        /// <summary>
        /// Create a new role
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<RoleDto>>> CreateRole([FromBody] CreateRoleDto createRoleDto)
        {
            var result = await _serviceManager.RoleService.CreateRoleAsync(createRoleDto);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(ApiResponse<RoleDto>.ErrorResponse(
                    "Failed to create role",
                    400,
                    errors.Select(e => new ApiError { Code = "ROLE_CREATE_FAILED", Message = e }).ToList()
                ));
            }

            var createdRole = await _serviceManager.RoleService.GetRoleByNameAsync(createRoleDto.RoleName);
            return CreatedAtAction(
                nameof(GetRoleById), 
                new { roleId = createdRole!.Id }, 
                ApiResponse<RoleDto>.SuccessResponse(createdRole, "Role created successfully")
            );
        }

        /// <summary>
        /// Update an existing role
        /// </summary>
        [HttpPut("{roleId}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateRole(string roleId, [FromBody] UpdateRoleDto updateRoleDto)
        {
            var result = await _serviceManager.RoleService.UpdateRoleAsync(roleId, updateRoleDto);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Failed to update role",
                    400,
                    errors.Select(e => new ApiError { Code = "ROLE_UPDATE_FAILED", Message = e }).ToList()
                ));
            }

            return Ok(ApiResponse<object>.SuccessResponse("Role updated successfully"));
        }

        /// <summary>
        /// Delete a role
        /// </summary>
        [HttpDelete("{roleId}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteRole(string roleId)
        {
            var result = await _serviceManager.RoleService.DeleteRoleAsync(roleId);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Failed to delete role",
                    400,
                    errors.Select(e => new ApiError { Code = "ROLE_DELETE_FAILED", Message = e }).ToList()
                ));
            }

            return Ok(ApiResponse<object>.SuccessResponse("Role deleted successfully"));
        }

        // ❌ REMOVED: Update user role endpoints
        // These should be handled by the domain services when creating users
    }
}