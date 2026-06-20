using University_Management_System.Application.Dtos.AuthDtos;
using Microsoft.AspNetCore.Identity;
using University_Management_System.Application.Dtos.RoleDtos;

namespace University_Management_System.Application.Contracts
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(string roleId);
        Task<RoleDto?> GetRoleByNameAsync(string roleName);
        Task<IdentityResult> CreateRoleAsync(CreateRoleDto createRoleDto);
        Task<IdentityResult> UpdateRoleAsync(string roleId, UpdateRoleDto updateRoleDto);
        Task<IdentityResult> DeleteRoleAsync(string roleId);
    }
}
