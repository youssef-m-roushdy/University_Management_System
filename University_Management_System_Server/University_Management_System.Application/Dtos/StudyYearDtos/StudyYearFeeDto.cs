using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Application.Dtos.StudyYearDtos
{
    public class StudyYearFeeDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int Level { get; set; }
        public string? DepartmentName { get; set; }
        public int? DepartmentId { get; set; }
    }
}