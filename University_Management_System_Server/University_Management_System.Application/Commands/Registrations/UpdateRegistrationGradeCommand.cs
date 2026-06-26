using MediatR;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Features.Registrations.Commands.UpdateRegistrationGrade
{
    public class UpdateRegistrationGradeCommand : IRequest<RegistrationDto>
    {
        public int RegistrationId { get; set; }
        public Grades? Grade { get; set; }
    }
}