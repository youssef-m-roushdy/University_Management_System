using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using University_Management_System.Application.Commands.Courses;
using University_Management_System.Application.Commands.CourseUploads;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Application.Dtos.CourseUploadDtos;
using University_Management_System.Application.Queries.CoursePrequisites;
using University_Management_System.Application.Queries.Courses;
using University_Management_System.Application.Queries.Registrations;
using University_Management_System.Domain.Enums;
using University_Management_System.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {

        private readonly IMediator _mediator;

        public CourseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateCourseDto courseDto)
        {
            var result = await _mediator.Send(new CreateCourseCommand(courseDto));
            //return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CourseQueries query)
        {
            var result = await _mediator.Send(new GetAllCoursesQuery(query));
            //return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("department/{departmentId}")]
        public async Task<IActionResult> DeparmentCourses(int departmentId, [FromQuery] DepartmentCourseQueries query)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _mediator.Send(new GetDepartmentCoursesQuery(departmentId, query));
            return Ok(result);
        }
     
        [Authorize]
        [HttpGet("prequisites/{courseId}")]
        public async Task<IActionResult> GetCoursePrequisites(int courseId)
        {
            var result = await _mediator.Send(new GetCoursePrequisitesQuery(courseId));
            return Ok(result);
        }

        [Authorize]
        [HttpGet("dependencies/{courseId}")]
        public async Task<IActionResult> GetCourseDependencies(int courseId)
        {
            var result = await _mediator.Send(new GetCourseDependenciesQuery(courseId));
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("status")]
        public async Task<IActionResult> UpdateCourseStatus([FromBody] UpdateCourseStatusDto updateCourseStatusDto)
        {
            var result = await _mediator.Send(new UpdateCourseStatusCommand(updateCourseStatusDto));
            return Ok(result);
        }

        //Get All
        //Get Basic Dep courses

    }
}