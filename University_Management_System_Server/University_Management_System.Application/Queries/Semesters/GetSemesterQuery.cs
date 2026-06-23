using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using University_Management_System.Application.Dtos.SemesterDtos;

namespace University_Management_System.Application.Queries.Semesters
{
    public class GetSemesterQuery : IRequest<SemesterDto>
    {
        public int Id { get; set; }
    }
}