using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Application.Commands.StudyYears;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Application.Queries.StudyYears;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudyYearController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudyYearController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllStudyYears()
        {
            var query = new GetAllStudyYearsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateStudyYear([FromBody] CreateStudyYearDto studyYearDto)
        {
            var command = new CreateStudyYearCommand(studyYearDto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}