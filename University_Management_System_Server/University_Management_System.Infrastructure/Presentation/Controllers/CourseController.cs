using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University_Management_System.Application.Commands.Courses;
using University_Management_System.Application.Dtos.CourseDtos;
using University_Management_System.Application.Queries.Courses;
using University_Management_System.Domain.Queries.CourseQueries;
using University_Management_System.Shared.Responses;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Security.Claims;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CourseController> _logger;

        public CourseController(IMediator mediator, ILogger<CourseController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // ────────────────────────────────────────────────────────────────────────
        // 1. GET /api/courses - Get all courses (Admin only)
        // ────────────────────────────────────────────────────────────────────────
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagedResponse<CourseDto>>> GetAllCourses([FromQuery] CourseFilterQueries query)
        {
            try
            {
                var result = await _mediator.Send(new GetCoursesQuery { Query = query });

                var response = PagedResponse<CourseDto>.SuccessResponse(
                    result.Data,
                    query.PageNumber,
                    query.PageSize,
                    result.TotalCount,
                    "Courses retrieved successfully"
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting courses");
                return StatusCode(500, PagedResponse<CourseDto>.ServerErrorResponse("An error occurred while retrieving courses"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // 2. GET /api/courses/{id} - Get course by ID (All authenticated users)
        // ────────────────────────────────────────────────────────────────────────
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<CourseDto>>> GetCourseById(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetCourseQuery { Id = id });

                if (result == null)
                    return NotFound(ApiResponse<CourseDto>.NotFoundResponse($"Course with ID '{id}' not found"));

                return Ok(ApiResponse<CourseDto>.SuccessResponse(result, "Course retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting course by ID: {Id}", id);
                return StatusCode(500, ApiResponse<CourseDto>.ServerErrorResponse("An error occurred while retrieving course"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // 3. GET /api/courses/department/{departmentId} - Get courses by department (All authenticated users)
        // ────────────────────────────────────────────────────────────────────────
        [HttpGet("department/{departmentId}")]
        [Authorize]
        public async Task<ActionResult<PagedResponse<CourseDto>>> GetDepartmentCourses(
            int departmentId,
            [FromQuery] CourseDepartmentQueries query)
        {
            try
            {
                var result = await _mediator.Send(new GetDepartmentCoursesQuery
                {
                    DepartmentId = departmentId,
                    Query = query
                });

                var response = PagedResponse<CourseDto>.SuccessResponse(
                    result.Data,
                    query.PageNumber,
                    query.PageSize,
                    result.TotalCount,
                    "Department courses retrieved successfully"
                );

                return Ok(response);
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(PagedResponse<CourseDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting department courses");
                return StatusCode(500, PagedResponse<CourseDto>.ServerErrorResponse(
                    "An error occurred while retrieving department courses"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // 6. POST /api/courses - Create course (Admin only)
        // ────────────────────────────────────────────────────────────────────────
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<CourseDto>>> CreateCourse([FromBody] CreateCourseDto dto)
        {
            try
            {
                var command = new CreateCourseCommand { Dto = dto };
                var result = await _mediator.Send(command);

                return StatusCode(201, ApiResponse<CourseDto>.SuccessResponse(result, "Course created successfully"));
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
                _logger.LogError(ex, "Error creating course");
                return StatusCode(500, ApiResponse<CourseDto>.ServerErrorResponse("An error occurred while creating course"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // 7. PUT /api/courses/{id} - Update course (Admin only)
        // ────────────────────────────────────────────────────────────────────────
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<CourseDto>>> UpdateCourse(int id, [FromBody] UpdateCourseDto dto)
        {
            try
            {
                var command = new UpdateCourseCommand
                {
                    Id = id,
                    Dto = dto
                };

                var result = await _mediator.Send(command);

                return Ok(ApiResponse<CourseDto>.SuccessResponse(result, "Course updated successfully"));
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
                _logger.LogError(ex, "Error updating course: {Id}", id);
                return StatusCode(500, ApiResponse<CourseDto>.ServerErrorResponse("An error occurred while updating course"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // 8. PATCH /api/courses/{id}/status - Update course status (Admin only)
        // ────────────────────────────────────────────────────────────────────────
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<CourseDto>>> UpdateCourseStatus(int id, [FromBody] UpdateCourseStatusDto dto)
        {
            try
            {
                var command = new UpdateCourseStatusCommand
                {
                    Id = id,
                    Status = dto.Status
                };

                var result = await _mediator.Send(command);

                return Ok(ApiResponse<CourseDto>.SuccessResponse(result, "Course status updated successfully"));
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
                _logger.LogError(ex, "Error updating course status: {Id}", id);
                return StatusCode(500, ApiResponse<CourseDto>.ServerErrorResponse("An error occurred while updating course status"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // 9. DELETE /api/courses/{id} - Delete course (Admin only)
        // ────────────────────────────────────────────────────────────────────────
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteCourse(int id)
        {
            try
            {
                await _mediator.Send(new DeleteCourseCommand { Id = id });

                return Ok(ApiResponse<object>.SuccessResponse("Course deleted successfully"));
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
                _logger.LogError(ex, "Error deleting course: {Id}", id);
                return StatusCode(500, ApiResponse<object>.ServerErrorResponse("An error occurred while deleting course"));
            }
        }

        // GET /api/courses/search?q=data+str&departmentId=1&maxResults=10
        [HttpGet("search")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseSearchResultDto>>>> SearchCourses(
            [FromQuery] string? q,
            [FromQuery] int? departmentId,
            [FromQuery] int? maxResults)
        {
            try
            {
                var query = new SearchCoursesQuery
                {
                    SearchTerm = q,
                    DepartmentId = departmentId,
                    MaxResults = maxResults ?? 20
                };

                var result = await _mediator.Send(query);

                return Ok(ApiResponse<IEnumerable<CourseSearchResultDto>>.SuccessResponse(
                    result,
                    "Courses retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching courses");
                return StatusCode(500, ApiResponse<IEnumerable<CourseSearchResultDto>>.ServerErrorResponse(
                    "An error occurred while searching courses"));
            }
        }
    }
}