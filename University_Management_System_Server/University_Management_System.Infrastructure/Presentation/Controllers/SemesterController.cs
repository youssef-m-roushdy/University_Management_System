using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Commands.Semesters;
using University_Management_System.Application.Dtos.SemesterDtos;
using University_Management_System.Application.Queries.Semesters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SemesterController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SemesterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //get study year semesters
        [HttpGet("{studyYearId}/study-year")]
        public async Task<IActionResult> GetStudyYearSemesters(int studyYearId)
        {
            var query = new GetStudyYearSemestersQuery(studyYearId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        
    }
}