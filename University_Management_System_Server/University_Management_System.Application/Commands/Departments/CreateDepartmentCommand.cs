using University_Management_System.Application.Dtos.DepartmentDtos;
using MediatR;
using University_Management_System.Shared.Respones;

namespace University_Management_System.Application.Commands.Departments
{
    public class CreateDepartmentCommand : IRequest<Response<DepartmentDto>>
    {
        public CreateDepartmentDto Department { get; set; }

        public CreateDepartmentCommand(CreateDepartmentDto department)
        {
            Department = department;
        }
    }
}