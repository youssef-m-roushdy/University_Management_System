using University_Management_System.Application.Dtos.DepartmentDtos;
using MediatR;
using University_Management_System.Shared.Respones;

namespace University_Management_System.Application.Commands.Departments
{
    public class UpdateDeparmentCommand : IRequest<Response<DepartmentDto>>
    {        
        public int Id { get; set; }
        public UpdateDepartmentDto Department { get; set; }

        public UpdateDeparmentCommand(int id, UpdateDepartmentDto department)
        {
            Id = id;
            Department = department;
        }
    }
}