using System;
using University_Management_System.Application.Dtos.SemesterDtos;
using MediatR;

namespace University_Management_System.Application.Commands.Semesters
{
    public class UpdateSemesterCommand : IRequest<SemesterDto>
    {
        public int Id { get; set; }
        public UpdateSemesterDto SemesterDto { get; set; } = null!;
    }
}