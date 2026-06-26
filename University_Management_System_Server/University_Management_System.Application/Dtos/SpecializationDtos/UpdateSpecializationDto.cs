using System;

namespace University_Management_System.Application.Dtos.SpecializationDtos
{
    public class UpdateSpecializationDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? DepartmentId { get; set; }
    }
}