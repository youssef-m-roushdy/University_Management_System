using System.Collections.Generic;
using MediatR;
using University_Management_System.Application.Dtos.RegistrationDtos;

namespace University_Management_System.Application.Features.Registrations.Commands.BulkUpdateRegistrationGrades
{
    public class BulkUpdateRegistrationGradesCommand : IRequest<IEnumerable<RegistrationDto>>
    {
        public List<UpdateRegistrationGradeDto> Updates { get; set; } = new();
    }
}