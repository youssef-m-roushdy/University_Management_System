using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using University_Management_System.Application.Dtos.SemesterDtos;
using University_Management_System.Domain.Queries.SemesterQueries;

namespace University_Management_System.Application.Queries.Semesters
{
    public class GetSemestersQuery : IRequest<(IEnumerable<SemesterDto> Data, int TotalCount)>
    {
        public SemesterFilterQueries Query { get; set; }
    }
}