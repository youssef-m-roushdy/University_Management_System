using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University_Management_System.Application.Commands.StudentStudyYears;
using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using University_Management_System.Application.Queries.StudentStudyYears;
using University_Management_System.Domain.Queries;
using University_Management_System.Shared.Responses;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentStudyYearController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StudentStudyYearController> _logger;

        public StudentStudyYearController(IMediator mediator, ILogger<StudentStudyYearController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/student-study-years/study-year/{studyYearId}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get all students in a study year (Admin, Instructor, Assistant)
        /// </summary>
        [HttpGet("study-year/{studyYearId}")]
        [Authorize(Roles = "Admin,Instructor,Assistant")]
        public async Task<ActionResult<PagedResponse<StudentStudyYearDto>>> GetStudentsByStudyYear(
            int studyYearId,
            [FromQuery] StudyYearStudentQueries query)
        {
            try
            {
                var result = await _mediator.Send(new GetStudentsByStudyYearQuery
                {
                    StudyYearId = studyYearId,
                    Query = query
                });

                var response = PagedResponse<StudentStudyYearDto>.SuccessResponse(
                    result.Data,
                    query.PageNumber,
                    query.PageSize,
                    result.TotalCount,
                    "Students retrieved successfully"
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting students for study year: {StudyYearId}", studyYearId);
                return StatusCode(500, PagedResponse<StudentStudyYearDto>.ServerErrorResponse(
                    "An error occurred while retrieving students"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/student-study-years/me
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get my study years journey (Student only)
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentStudyYearDto>>>> GetMyStudyYears()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<IEnumerable<StudentStudyYearDto>>.UnauthorizedResponse("Invalid token"));

                var result = await _mediator.Send(new GetStudentStudyYearsQuery { StudentId = userId });

                return Ok(ApiResponse<IEnumerable<StudentStudyYearDto>>.SuccessResponse(
                    result,
                    "Your study years retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting my study years");
                return StatusCode(500, ApiResponse<IEnumerable<StudentStudyYearDto>>.ServerErrorResponse(
                    "An error occurred while retrieving your study years"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/student-study-years/me/current
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get my current study year (Student only)
        /// </summary>
        [HttpGet("me/current")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<StudentStudyYearDto>>> GetMyCurrentStudyYear()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<StudentStudyYearDto>.UnauthorizedResponse("Invalid token"));

                var result = await _mediator.Send(new GetStudentCurrentStudyYearQuery { StudentId = userId });

                if (result == null)
                    return NotFound(ApiResponse<StudentStudyYearDto>.NotFoundResponse("No current study year found"));

                return Ok(ApiResponse<StudentStudyYearDto>.SuccessResponse(
                    result,
                    "Your current study year retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting my current study year");
                return StatusCode(500, ApiResponse<StudentStudyYearDto>.ServerErrorResponse(
                    "An error occurred while retrieving your current study year"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/student-study-years
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Enroll a student in a study year (Admin only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<StudentStudyYearDto>>> EnrollStudent(
            [FromBody] CreateStudentStudyYearDto dto)
        {
            try
            {
                var result = await _mediator.Send(new CreateStudentStudyYearCommand { Dto = dto });

                return CreatedAtAction(
                    nameof(GetStudentsByStudyYear),
                    new { studyYearId = result.StudyYearId },
                    ApiResponse<StudentStudyYearDto>.SuccessResponse(result, "Student enrolled successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<StudentStudyYearDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<StudentStudyYearDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enrolling student");
                return StatusCode(500, ApiResponse<StudentStudyYearDto>.ServerErrorResponse(
                    "An error occurred while enrolling student"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // PUT /api/student-study-years/{id}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Update enrollment (activate/deactivate) (Admin only)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<StudentStudyYearDto>>> UpdateEnrollment(
            int id,
            [FromBody] UpdateStudentStudyYearDto dto)
        {
            try
            {
                var result = await _mediator.Send(new UpdateStudentStudyYearCommand
                {
                    Id = id,
                    Dto = dto
                });

                return Ok(ApiResponse<StudentStudyYearDto>.SuccessResponse(result, "Enrollment updated successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<StudentStudyYearDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<StudentStudyYearDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating enrollment: {Id}", id);
                return StatusCode(500, ApiResponse<StudentStudyYearDto>.ServerErrorResponse(
                    "An error occurred while updating enrollment"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // DELETE /api/student-study-years/{id}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Remove enrollment (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteEnrollment(int id)
        {
            try
            {
                await _mediator.Send(new DeleteStudentStudyYearCommand { Id = id });

                return Ok(ApiResponse<object>.SuccessResponse("Enrollment deleted successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting enrollment: {Id}", id);
                return StatusCode(500, ApiResponse<object>.ServerErrorResponse(
                    "An error occurred while deleting enrollment"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/student-study-years/student/{studentId}/timeline
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get student's study year timeline (Admin, Instructor, Assistant)
        /// </summary>
        [HttpGet("student/{studentId}/timeline")]
        [Authorize(Roles = "Admin,Instructor,Assistant")]
        public async Task<ActionResult<ApiResponse<StudentStudyYearTimelineDto>>> GetStudentTimeline(string studentId)
        {
            try
            {
                var result = await _mediator.Send(new GetStudentStudyYearTimelineQuery { StudentId = studentId });

                return Ok(ApiResponse<StudentStudyYearTimelineDto>.SuccessResponse(
                    result,
                    "Student timeline retrieved successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<StudentStudyYearTimelineDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student timeline: {StudentId}", studentId);
                return StatusCode(500, ApiResponse<StudentStudyYearTimelineDto>.ServerErrorResponse(
                    "An error occurred while retrieving student timeline"));
            }
        }
    }
}