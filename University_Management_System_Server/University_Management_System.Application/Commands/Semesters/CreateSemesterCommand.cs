using System;
using University_Management_System.Application.Dtos.SemesterDtos;
using MediatR;

namespace University_Management_System.Application.Commands.Semesters
{
    public class CreateSemesterCommand : IRequest<SemesterDto>
    {
        public CreateSemesterDto SemesterDto { get; set; } = null!;
    }
}