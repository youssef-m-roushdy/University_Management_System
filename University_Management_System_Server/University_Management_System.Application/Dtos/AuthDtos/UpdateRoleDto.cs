using System.ComponentModel.DataAnnotations;

namespace University_Management_System.Application.Dtos.AuthDtos
{
    public record UpdateRoleDto
    {
        [Required]
        public string NewRoleName { get; set; } = string.Empty;
    }
}
