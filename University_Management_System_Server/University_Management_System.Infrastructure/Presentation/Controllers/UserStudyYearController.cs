using System.Security.Claims;
using University_Management_System.Application.Commands.StudentStudyYears;
using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using University_Management_System.Application.Queries.StudentStudyYears;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentStudyYearController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentStudyYearController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Assign a study year to a Student (Admin/Instructor only).
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> CreateStudentStudyYear(CreateStudentStudyYearDto dto)
        {
            var command = new CreateStudentStudyYearCommand(dto);
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Update a Student's study year record (e.g., promote level, set current).
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> UpdateStudentStudyYear(int id, UpdateStudentStudyYearDto dto)
        {
            var command = new UpdateStudentStudyYearCommand(id, dto);
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Get all study years for the currently authenticated Student.
        /// </summary>
        [HttpGet("my-study-years")]
        public async Task<IActionResult> GetMyStudyYears()
        {
            var StudentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(StudentId))
                return Unauthorized();

            var query = new GetStudentStudyYearsQuery(StudentId);
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Get the full study year timeline for the authenticated Student (from enrollment to graduation).
        /// </summary>
        [HttpGet("my-timeline")]
        public async Task<IActionResult> GetMyTimeline()
        {
            var StudentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(StudentId))
                return Unauthorized();

            var query = new GetStudentStudyYearTimelineQuery(StudentId);
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Get the current active study year for the authenticated Student.
        /// </summary>
        [HttpGet("my-current")]
        public async Task<IActionResult> GetMyCurrentStudyYear()
        {
            var StudentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(StudentId))
                return Unauthorized();

            var query = new GetCurrentStudentStudyYearQuery(StudentId);
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Get all study years for a specific Student (Admin/Instructor only).
        /// </summary>
        [HttpGet("Student/{StudentId}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> GetStudentStudyYears(string StudentId)
        {
            var query = new GetStudentStudyYearsQuery(StudentId);
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Get timeline for a specific Student (Admin/Instructor only).
        /// </summary>
        [HttpGet("Student/{StudentId}/timeline")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> GetStudentTimeline(string StudentId)
        {
            var query = new GetStudentStudyYearTimelineQuery(StudentId);
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // Assign all Student that are students that are not graduated yet from first year to second year, from second year to third year, and from third year to fourth year (Admin only).
        [Authorize(Roles = "Admin")]
        [HttpPost("promote-specific-students")]
        public async Task<IActionResult> PromoteAllStudents(List<string> academicCodes)
        {
            var command = new PromoteSpecificStudentsCommand(academicCodes);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("promote-student/{academicCode}")]
        public async Task<IActionResult> PromoteStudent(string academicCode)
        {
            var command = new PromoteStudentCommand(academicCode);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
