using MediatR;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Commands.Registrations
{
    public class UpdateRegistrationStatusCommand : IRequest<RegistrationDto>
    {
        public int RegistrationId { get; set; }
        public RegistrationStatus Status { get; set; }
        public string? Reason { get; set; }
        public string StudentId { get; set; } = string.Empty;
    }
}