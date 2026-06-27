using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University_Management_System.Application.Dtos.SemesterGPADtos;
using University_Management_System.Application.Queries.SemesterGPAs;
using University_Management_System.Domain.Queries.SemesterGPAQueries;
using University_Management_System.Shared.Responses;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SemesterGPAController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SemesterGPAController> _logger;

        public SemesterGPAController(IMediator mediator, ILogger<SemesterGPAController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // ────────────────────────────────────────────────────────────────────────
        // 1. GET /api/semestergpa/student/{id} - Get student's semester GPA (Student only)
        // ────────────────────────────────────────────────────────────────────────
        [HttpGet("student/{id}")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<ApiResponse<SemesterGPADto>>> GetStudentSemesterGPA(int id)
        {
            try
            {
                var userId = User.FindFirst("uid")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<SemesterGPADto>.ErrorResponse("User not authenticated"));

                var query = new GetStudentSemesterGPAQuery
                {
                    SemesterGPAId = id,
                    UserId = userId
                };

                var result = await _mediator.Send(query);

                return Ok(ApiResponse<SemesterGPADto>.SuccessResponse(result, "Semester GPA retrieved successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<SemesterGPADto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ForbiddenException ex)
            {
                return Forbid();
            }
            catch (FluentValidation.ValidationException ex)
            {
                var errors = ex.Errors.Select(e => e.ErrorMessage);
                return BadRequest(ApiResponse<SemesterGPADto>.ErrorResponse(string.Join(", ", errors)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student semester GPA: {Id}", id);
                return StatusCode(500, ApiResponse<SemesterGPADto>.ServerErrorResponse("An error occurred while retrieving semester GPA"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // 2. GET /api/semestergpa/studyyear/{studyYearId} - Get semester GPAs by study year (Admin/Instructor/Assistant only)
        // ────────────────────────────────────────────────────────────────────────
        [HttpGet("studyyear/{studyYearId}")]
        [Authorize(Roles = "Admin,Instructor,Assistant")]
        public async Task<ActionResult<PagedResponse<SemesterGPADto>>> GetStudyYearSemesterGPAs(
            int studyYearId,
            [FromQuery] semesterGPAStudyYearQueries filter)
        {
            try
            {
                var query = new GetStudyYearSemesterGPAsQuery
                {
                    StudyYearId = studyYearId,
                    Filter = filter
                };

                var result = await _mediator.Send(query);

                var response = PagedResponse<SemesterGPADto>.SuccessResponse(
                    result.Data,
                    filter.PageNumber,
                    filter.PageSize,
                    result.TotalCount,
                    "Study year semester GPAs retrieved successfully"
                );

                return Ok(response);
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(PagedResponse<SemesterGPADto>.NotFoundResponse(ex.Message));
            }
            catch (FluentValidation.ValidationException ex)
            {
                var errors = ex.Errors.Select(e => e.ErrorMessage);
                return BadRequest(PagedResponse<SemesterGPADto>.ErrorResponse(string.Join(", ", errors)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting study year semester GPAs: {StudyYearId}", studyYearId);
                return StatusCode(500, PagedResponse<SemesterGPADto>.ServerErrorResponse("An error occurred while retrieving study year semester GPAs"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // 3. GET /api/semestergpa/semester/{semesterId} - Get semester GPAs by semester (Admin/Instructor/Assistant only)
        // ────────────────────────────────────────────────────────────────────────
        [HttpGet("semester/{semesterId}")]
        [Authorize(Roles = "Admin,Instructor,Assistant")]
        public async Task<ActionResult<PagedResponse<SemesterGPADto>>> GetSemesterGPAsBySemester(
            int semesterId,
            [FromQuery] SemesterGPAFilterInSemesterQueries filter)
        {
            try
            {
                var query = new GetSemesterGPAsBySemesterQuery
                {
                    SemesterId = semesterId,
                    Filter = filter
                };

                var result = await _mediator.Send(query);

                var response = PagedResponse<SemesterGPADto>.SuccessResponse(
                    result.Data,
                    filter.PageNumber,
                    filter.PageSize,
                    result.TotalCount,
                    "Semester GPAs retrieved successfully"
                );

                return Ok(response);
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(PagedResponse<SemesterGPADto>.NotFoundResponse(ex.Message));
            }
            catch (FluentValidation.ValidationException ex)
            {
                var errors = ex.Errors.Select(e => e.ErrorMessage);
                return BadRequest(PagedResponse<SemesterGPADto>.ErrorResponse(string.Join(", ", errors)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting semester GPAs by semester: {SemesterId}", semesterId);
                return StatusCode(500, PagedResponse<SemesterGPADto>.ServerErrorResponse("An error occurred while retrieving semester GPAs"));
            }
        }
    }
}