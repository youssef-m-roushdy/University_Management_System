using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.DepartmentDtos.FeeDtos
{
    public class UpdateFeeDto
    {
        public FeeType Type { get; set; }
        public Levels Level { get; set; } // first year, second year, etc.
        public string? Description { get; set; }
        public decimal Amount { get; set; }
    }
}