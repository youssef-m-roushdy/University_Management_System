using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University_Management_System.Application.Commands.SpecializationCourses;
using University_Management_System.Application.Dtos.SpecializationCourseDtos;
using University_Management_System.Application.Queries.SpecializationCourses;
using University_Management_System.Domain.Queries.SpecializationCourseQueries;
using University_Management_System.Shared.Responses;
using Microsoft.Extensions.Logging;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecializationCourseController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SpecializationCourseController> _logger;

        public SpecializationCourseController(IMediator mediator, ILogger<SpecializationCourseController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/specialization-courses
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get all specialization courses with filters (Admin only)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagedResponse<SpecializationCourseDto>>> GetAll(
            [FromQuery] SpecializationCourseFilterQueries query)
        {
            try
            {
                var result = await _mediator.Send(new GetSpecializationCoursesQuery { Query = query });

                var response = PagedResponse<SpecializationCourseDto>.SuccessResponse(
                    result.Data,
                    query.PageNumber,
                    query.PageSize,
                    result.TotalCount,
                    "Specialization courses retrieved successfully"
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting specialization courses");
                return StatusCode(500, PagedResponse<SpecializationCourseDto>.ServerErrorResponse(
                    "An error occurred while retrieving specialization courses"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/specialization-courses/specialization/{specializationId}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get courses by specialization (Admin, Instructor, Assistant, Student)
        /// </summary>
        [HttpGet("specialization/{specializationId}")]
        [Authorize(Roles = "Admin,Instructor,Assistant,Student")]
        public async Task<ActionResult<PagedResponse<SpecializationCourseDto>>> GetBySpecialization(
            int specializationId,
            [FromQuery] CourseFilterInSpecailizationQueries query)
        {
            try
            {
                var result = await _mediator.Send(new GetSpecializationCoursesBySpecializationQuery
                {
                    SpecializationId = specializationId,
                    Query = query
                });

                var response = PagedResponse<SpecializationCourseDto>.SuccessResponse(
                    result.Data,
                    query.PageNumber,
                    query.PageSize,
                    result.TotalCount,
                    "Specialization courses retrieved successfully"
                );

                return Ok(response);
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(PagedResponse<SpecializationCourseDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting specialization courses");
                return StatusCode(500, PagedResponse<SpecializationCourseDto>.ServerErrorResponse(
                    "An error occurred while retrieving specialization courses"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/specialization-courses/course/{courseId}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get specializations by course (Admin only)
        /// </summary>
        [HttpGet("course/{courseId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SpecializationCourseDto>>>> GetByCourse(int courseId)
        {
            try
            {
                var result = await _mediator.Send(new GetSpecializationCoursesByCourseQuery { CourseId = courseId });

                return Ok(ApiResponse<IEnumerable<SpecializationCourseDto>>.SuccessResponse(
                    result,
                    "Specializations for course retrieved successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<IEnumerable<SpecializationCourseDto>>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting specializations by course: {CourseId}", courseId);
                return StatusCode(500, ApiResponse<IEnumerable<SpecializationCourseDto>>.ServerErrorResponse(
                    "An error occurred while retrieving specializations for course"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/specialization-courses
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Create a specialization course mapping (Admin only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<SpecializationCourseDto>>> Create(
            [FromBody] CreateSpecializationCourseDto dto)
        {
            try
            {
                var command = new CreateSpecializationCourseCommand { Dto = dto };
                var result = await _mediator.Send(command);

                return StatusCode(201, ApiResponse<SpecializationCourseDto>.SuccessResponse(
                    result,
                    "Specialization course created successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<SpecializationCourseDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<SpecializationCourseDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating specialization course");
                return StatusCode(500, ApiResponse<SpecializationCourseDto>.ServerErrorResponse(
                    "An error occurred while creating specialization course"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/specialization-courses/bulk
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Create multiple specialization course mappings (Admin only)
        /// </summary>
        [HttpPost("bulk")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SpecializationCourseDto>>>> CreateBulk(
            [FromBody] CreateSpecializationCourseBulkDto dto)
        {
            try
            {
                var command = new CreateSpecializationCourseBulkCommand { Dto = dto };
                var result = await _mediator.Send(command);

                return StatusCode(201, ApiResponse<IEnumerable<SpecializationCourseDto>>.SuccessResponse(
                    result,
                    "Specialization courses created successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<IEnumerable<SpecializationCourseDto>>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<IEnumerable<SpecializationCourseDto>>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating specialization courses bulk");
                return StatusCode(500, ApiResponse<IEnumerable<SpecializationCourseDto>>.ServerErrorResponse(
                    "An error occurred while creating specialization courses"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // PUT /api/specialization-courses/{specializationId}/{courseId}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Update a specialization course mapping (Admin only)
        /// </summary>
        [HttpPut("{specializationId}/{courseId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<SpecializationCourseDto>>> Update(
            int specializationId,
            int courseId,
            [FromBody] UpdateSpecializationCourseDto dto)
        {
            try
            {
                var command = new UpdateSpecializationCourseCommand
                {
                    SpecializationId = specializationId,
                    CourseId = courseId,
                    Dto = dto
                };

                var result = await _mediator.Send(command);

                return Ok(ApiResponse<SpecializationCourseDto>.SuccessResponse(
                    result,
                    "Specialization course updated successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<SpecializationCourseDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<SpecializationCourseDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating specialization course");
                return StatusCode(500, ApiResponse<SpecializationCourseDto>.ServerErrorResponse(
                    "An error occurred while updating specialization course"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // DELETE /api/specialization-courses/{specializationId}/{courseId}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Delete a specialization course mapping (Admin only)
        /// </summary>
        [HttpDelete("{specializationId}/{courseId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(
            int specializationId,
            int courseId)
        {
            try
            {
                var command = new DeleteSpecializationCourseCommand
                {
                    SpecializationId = specializationId,
                    CourseId = courseId
                };

                await _mediator.Send(command);

                return Ok(ApiResponse<object>.SuccessResponse("Specialization course deleted successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting specialization course");
                return StatusCode(500, ApiResponse<object>.ServerErrorResponse(
                    "An error occurred while deleting specialization course"));
            }
        }
    }
}