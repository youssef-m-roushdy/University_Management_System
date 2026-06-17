using System.ComponentModel.DataAnnotations;

namespace University_Management_System.Application.Dtos.AuthDtos
{
    public record CreateRoleDto
    {
        [Required]
        public string RoleName { get; set; } = string.Empty;
    }
}
