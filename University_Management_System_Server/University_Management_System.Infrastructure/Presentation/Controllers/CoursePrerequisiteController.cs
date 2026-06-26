using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University_Management_System.Application.Commands.CoursePrerequisites;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Application.Dtos.CoursePrerequisiteDtos;
using University_Management_System.Shared.Responses;
using Microsoft.Extensions.Logging;
using University_Management_System.Application.Queries.CoursePrequisites;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CoursePrerequisiteController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CoursePrerequisiteController> _logger;

        public CoursePrerequisiteController(IMediator mediator, ILogger<CoursePrerequisiteController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET ENDPOINTS (All authenticated users)
        // ────────────────────────────────────────────────────────────────────────

        // GET /api/course-prerequisites/{id}/prerequisites
        [HttpGet("{id}/prerequisites")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseDto>>>> GetCoursePrerequisites(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetCoursePrerequisitesQuery { CourseId = id });

                return Ok(ApiResponse<IEnumerable<CourseDto>>.SuccessResponse(
                    result,
                    "Course prerequisites retrieved successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<IEnumerable<CourseDto>>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting course prerequisites: {Id}", id);
                return StatusCode(500, ApiResponse<IEnumerable<CourseDto>>.ServerErrorResponse(
                    "An error occurred while retrieving course prerequisites"));
            }
        }

        // GET /api/course-prerequisites/{id}/dependencies
        [HttpGet("{id}/dependencies")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseDto>>>> GetCourseDependencies(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetCourseDependenciesQuery { CourseId = id });

                return Ok(ApiResponse<IEnumerable<CourseDto>>.SuccessResponse(
                    result,
                    "Course dependencies retrieved successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<IEnumerable<CourseDto>>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting course dependencies: {Id}", id);
                return StatusCode(500, ApiResponse<IEnumerable<CourseDto>>.ServerErrorResponse(
                    "An error occurred while retrieving course dependencies"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // PREREQUISITE ENDPOINTS (Admin only)
        // ────────────────────────────────────────────────────────────────────────

        // POST /api/course-prerequisites/prerequisites
        [HttpPost("prerequisites")]
        public async Task<ActionResult<ApiResponse<CourseDto>>> CreatePrerequisite(
            [FromBody] CreateCoursePrerequisiteDto dto)
        {
            try
            {
                var command = new CreateCoursePrerequisiteCommand { Dto = dto };
                var result = await _mediator.Send(command);

                return StatusCode(201, ApiResponse<CourseDto>.SuccessResponse(
                    result,
                    "Prerequisite created successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<CourseDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<CourseDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating prerequisite");
                return StatusCode(500, ApiResponse<CourseDto>.ServerErrorResponse(
                    "An error occurred while creating prerequisite"));
            }
        }

        // POST /api/course-prerequisites/prerequisites/bulk
        [HttpPost("prerequisites/bulk")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseDto>>>> CreatePrerequisitesBulk(
            [FromBody] CreateCoursePrerequisiteBulkDto dto)
        {
            try
            {
                var command = new CreateCoursePrerequisiteBulkCommand { Dto = dto };
                var result = await _mediator.Send(command);

                return StatusCode(201, ApiResponse<IEnumerable<CourseDto>>.SuccessResponse(
                    result,
                    "Prerequisites created successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<IEnumerable<CourseDto>>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<IEnumerable<CourseDto>>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating prerequisites bulk");
                return StatusCode(500, ApiResponse<IEnumerable<CourseDto>>.ServerErrorResponse(
                    "An error occurred while creating prerequisites"));
            }
        }

        // ✅ DELETE /api/course-prerequisites/prerequisites/{courseId}/{prerequisiteCourseId}
        [HttpDelete("prerequisites/{courseId}/{prerequisiteCourseId}")]
        public async Task<ActionResult<ApiResponse<object>>> DeletePrerequisite(
            int courseId,
            int prerequisiteCourseId)
        {
            try
            {
                var command = new DeleteCoursePrerequisiteCommand
                {
                    CourseId = courseId,
                    PrerequisiteCourseId = prerequisiteCourseId
                };

                await _mediator.Send(command);

                return Ok(ApiResponse<object>.SuccessResponse("Prerequisite deleted successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting prerequisite");
                return StatusCode(500, ApiResponse<object>.ServerErrorResponse(
                    "An error occurred while deleting prerequisite"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // DEPENDENCY ENDPOINTS (Admin only)
        // ────────────────────────────────────────────────────────────────────────

        // POST /api/course-prerequisites/dependencies
        [HttpPost("dependencies")]
        public async Task<ActionResult<ApiResponse<CourseDto>>> CreateDependency(
            [FromBody] CreateCourseDependencyDto dto)
        {
            try
            {
                var command = new CreateCourseDependencyCommand { Dto = dto };
                var result = await _mediator.Send(command);

                return StatusCode(201, ApiResponse<CourseDto>.SuccessResponse(
                    result,
                    "Dependency created successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<CourseDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<CourseDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating dependency");
                return StatusCode(500, ApiResponse<CourseDto>.ServerErrorResponse(
                    "An error occurred while creating dependency"));
            }
        }

        // POST /api/course-prerequisites/dependencies/bulk
        [HttpPost("dependencies/bulk")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseDto>>>> CreateDependenciesBulk(
            [FromBody] CreateCourseDependencyBulkDto dto)
        {
            try
            {
                var command = new CreateCourseDependencyBulkCommand { Dto = dto };
                var result = await _mediator.Send(command);

                return StatusCode(201, ApiResponse<IEnumerable<CourseDto>>.SuccessResponse(
                    result,
                    "Dependencies created successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<IEnumerable<CourseDto>>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<IEnumerable<CourseDto>>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating dependencies bulk");
                return StatusCode(500, ApiResponse<IEnumerable<CourseDto>>.ServerErrorResponse(
                    "An error occurred while creating dependencies"));
            }
        }

        // ✅ DELETE /api/course-prerequisites/dependencies/{courseId}/{dependencyCourseId}
        [HttpDelete("dependencies/{courseId}/{dependencyCourseId}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteDependency(
            int courseId,
            int dependencyCourseId)
        {
            try
            {
                var command = new DeleteCourseDependencyCommand
                {
                    CourseId = courseId,
                    DependencyCourseId = dependencyCourseId
                };

                await _mediator.Send(command);

                return Ok(ApiResponse<object>.SuccessResponse("Dependency deleted successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting dependency");
                return StatusCode(500, ApiResponse<object>.ServerErrorResponse(
                    "An error occurred while deleting dependency"));
            }
        }
    }
}