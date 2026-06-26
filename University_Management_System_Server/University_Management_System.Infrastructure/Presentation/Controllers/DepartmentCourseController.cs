using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University_Management_System.Application.Commands.DepartmentCourses;
using University_Management_System.Application.Dtos.DepartmentCourseDtos;
using University_Management_System.Application.Queries.DepartmentCourses;
using University_Management_System.Domain.Queries.DepartmentCourseQueries;
using University_Management_System.Shared.Responses;
using Microsoft.Extensions.Logging;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class DepartmentCourseController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DepartmentCourseController> _logger;

        public DepartmentCourseController(IMediator mediator, ILogger<DepartmentCourseController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/department-courses/all
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get all department courses with filters (Admin only)
        /// </summary>
        [HttpGet("all")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DepartmentCourseDto>>>> GetAllDepartmentCourses(
            [FromQuery] CourseFilterInDepartmentQueries query)
        {
            try
            {
                var result = await _mediator.Send(new GetAllDepartmentCoursesQuery { Query = query });

                return Ok(ApiResponse<IEnumerable<DepartmentCourseDto>>.SuccessResponse(
                    result.Data,
                    "Department courses retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all department courses");
                return StatusCode(500, ApiResponse<IEnumerable<DepartmentCourseDto>>.ServerErrorResponse(
                    "An error occurred while retrieving department courses"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/department-courses/department/{departmentId}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get courses by department (Admin, Instructor, Assistant, Student)
        /// </summary>
        [HttpGet("department/{departmentId}")]
        [Authorize(Roles = "Admin,Instructor,Assistant,Student")]
        public async Task<ActionResult<PagedResponse<DepartmentCourseDto>>> GetDepartmentCourses(
            int departmentId,
            [FromQuery] DepartmentCourseFilterQueries query)
        {
            try
            {
                var result = await _mediator.Send(new GetDepartmentCoursesQuery
                {
                    DepartmentId = departmentId,
                    Query = query
                });

                var response = PagedResponse<DepartmentCourseDto>.SuccessResponse(
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
                return NotFound(PagedResponse<DepartmentCourseDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting department courses");
                return StatusCode(500, PagedResponse<DepartmentCourseDto>.ServerErrorResponse(
                    "An error occurred while retrieving department courses"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/department-courses
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Create a department course mapping (Admin only)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<DepartmentCourseDto>>> CreateDepartmentCourse(
            [FromBody] CreateDepartmentCourseDto dto)
        {
            try
            {
                var command = new CreateDepartmentCourseCommand { Dto = dto };
                var result = await _mediator.Send(command);

                return StatusCode(201, ApiResponse<DepartmentCourseDto>.SuccessResponse(
                    result,
                    "Department course created successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<DepartmentCourseDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<DepartmentCourseDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating department course");
                return StatusCode(500, ApiResponse<DepartmentCourseDto>.ServerErrorResponse(
                    "An error occurred while creating department course"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/department-courses/bulk
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Create multiple department course mappings (Admin only)
        /// </summary>
        [HttpPost("bulk")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DepartmentCourseDto>>>> CreateDepartmentCoursesBulk(
            [FromBody] CreateDepartmentCourseBulkDto dto)
        {
            try
            {
                var command = new CreateDepartmentCourseBulkCommand { Dto = dto };
                var result = await _mediator.Send(command);

                return StatusCode(201, ApiResponse<IEnumerable<DepartmentCourseDto>>.SuccessResponse(
                    result,
                    "Department courses created successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<IEnumerable<DepartmentCourseDto>>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<IEnumerable<DepartmentCourseDto>>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating department courses bulk");
                return StatusCode(500, ApiResponse<IEnumerable<DepartmentCourseDto>>.ServerErrorResponse(
                    "An error occurred while creating department courses"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // PUT /api/department-courses/{departmentId}/{courseId}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Update a department course mapping (Admin only)
        /// </summary>
        [HttpPut("{departmentId}/{courseId}")]
        public async Task<ActionResult<ApiResponse<DepartmentCourseDto>>> UpdateDepartmentCourse(
            int departmentId,
            int courseId,
            [FromBody] UpdateDepartmentCourseDto dto)
        {
            try
            {
                var command = new UpdateDepartmentCourseCommand
                {
                    DepartmentId = departmentId,
                    CourseId = courseId,
                    Dto = dto
                };

                var result = await _mediator.Send(command);

                return Ok(ApiResponse<DepartmentCourseDto>.SuccessResponse(
                    result,
                    "Department course updated successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<DepartmentCourseDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<DepartmentCourseDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating department course");
                return StatusCode(500, ApiResponse<DepartmentCourseDto>.ServerErrorResponse(
                    "An error occurred while updating department course"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // DELETE /api/department-courses/{departmentId}/{courseId}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Delete a department course mapping (Admin only)
        /// </summary>
        [HttpDelete("{departmentId}/{courseId}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteDepartmentCourse(
            int departmentId,
            int courseId)
        {
            try
            {
                var command = new DeleteDepartmentCourseCommand
                {
                    DepartmentId = departmentId,
                    CourseId = courseId
                };

                await _mediator.Send(command);

                return Ok(ApiResponse<object>.SuccessResponse("Department course deleted successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting department course");
                return StatusCode(500, ApiResponse<object>.ServerErrorResponse(
                    "An error occurred while deleting department course"));
            }
        }
    }
}