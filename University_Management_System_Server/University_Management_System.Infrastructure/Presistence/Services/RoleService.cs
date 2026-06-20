using University_Management_System.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Application.Contracts;
using University_Management_System.Application.Dtos.AuthDtos;
using University_Management_System.Application.Dtos.RoleDtos;

namespace University_Management_System.Infrastructure.Presistence.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleService(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles.Select(r => new RoleDto { Id = r.Id, Name = r.Name! });
        }

        public async Task<RoleDto?> GetRoleByIdAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return null;
            
            return new RoleDto { Id = role.Id, Name = role.Name! };
        }

        public async Task<RoleDto?> GetRoleByNameAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return null;
            
            return new RoleDto { Id = role.Id, Name = role.Name! };
        }

        public async Task<IdentityResult> CreateRoleAsync(CreateRoleDto createRoleDto)
        {
            var roleExists = await _roleManager.RoleExistsAsync(createRoleDto.RoleName);
            if (roleExists)
            {
                return IdentityResult.Failed(new IdentityError 
                { 
                    Description = $"Role '{createRoleDto.RoleName}' already exists." 
                });
            }

            var role = new Role { Name = createRoleDto.RoleName };
            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> UpdateRoleAsync(string roleId, UpdateRoleDto updateRoleDto)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError 
                { 
                    Description = $"Role with ID '{roleId}' not found." 
                });
            }

            role.Name = updateRoleDto.NewRoleName;
            return await _roleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError 
                { 
                    Description = $"Role with ID '{roleId}' not found." 
                });
            }

            return await _roleManager.DeleteAsync(role);
        }
    }
}