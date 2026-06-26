using System;

namespace University_Management_System.Application.Dtos.SpecializationDtos
{
    public class CreateSpecializationDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
    }
}