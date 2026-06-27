using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University_Management_System.Application.Commands.AcademicSchedules;
using University_Management_System.Application.Dtos.AcademicScheduleDtos;
using University_Management_System.Application.Queries.AcademicSchedules;
using University_Management_System.Domain.Queries.AcademicScheduleQueries;
using University_Management_System.Shared.Responses;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace University_Management_System.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AcademicScheduleController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AcademicScheduleController> _logger;

        public AcademicScheduleController(IMediator mediator, ILogger<AcademicScheduleController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/academic-schedules/department/{departmentId}/semester/{semesterId}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get academic schedules by department and semester (All authenticated users)
        /// </summary>
        [HttpGet("department/{departmentId}/semester/{semesterId}")]
        [Authorize]
        public async Task<ActionResult<PagedResponse<AcademicScheduleDto>>> GetByDepartmentAndSemester(
            int departmentId,
            int semesterId,
            [FromQuery] AcademicScheduleDepartmentSemesterQueries? filter = null)
        {
            try
            {
                var result = await _mediator.Send(new GetAcademicSchedulesByDepartmentAndSemesterQuery
                {
                    DepartmentId = departmentId,
                    SemesterId = semesterId,
                    Filter = filter
                });

                var response = PagedResponse<AcademicScheduleDto>.SuccessResponse(
                    result.Data,
                    filter?.PageNumber ?? 1,
                    filter?.PageSize ?? 10,
                    result.TotalCount,
                    "Academic schedules retrieved successfully"
                );

                return Ok(response);
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(PagedResponse<AcademicScheduleDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting academic schedules");
                return StatusCode(500, PagedResponse<AcademicScheduleDto>.ServerErrorResponse(
                    "An error occurred while retrieving academic schedules"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/academic-schedules/department/{departmentId}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get all academic schedules by department (Admin only)
        /// </summary>
        [HttpGet("department/{departmentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagedResponse<AcademicScheduleDto>>> GetByDepartment(
            int departmentId,
            [FromQuery] AcademicScheduleSemesterQueries? filter = null)
        {
            try
            {
                var result = await _mediator.Send(new GetAcademicSchedulesByDepartmentQuery
                {
                    DepartmentId = departmentId,
                    Filter = filter
                });

                var response = PagedResponse<AcademicScheduleDto>.SuccessResponse(
                    result.Data,
                    filter?.PageNumber ?? 1,
                    filter?.PageSize ?? 10,
                    result.TotalCount,
                    "Department academic schedules retrieved successfully"
                );

                return Ok(response);
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(PagedResponse<AcademicScheduleDto>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting department academic schedules");
                return StatusCode(500, PagedResponse<AcademicScheduleDto>.ServerErrorResponse(
                    "An error occurred while retrieving department academic schedules"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // GET /api/academic-schedules/{id}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Get academic schedule by ID (All authenticated users)
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<AcademicScheduleDto>>> GetById(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetAcademicScheduleQuery
                {
                    Id = id
                });

                if (result == null)
                    return NotFound(ApiResponse<AcademicScheduleDto>.NotFoundResponse($"Academic schedule with ID '{id}' not found"));

                return Ok(ApiResponse<AcademicScheduleDto>.SuccessResponse(
                    result,
                    "Academic schedule retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting academic schedule by ID: {Id}", id);
                return StatusCode(500, ApiResponse<AcademicScheduleDto>.ServerErrorResponse(
                    "An error occurred while retrieving academic schedule"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // POST /api/academic-schedules
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Create a new academic schedule (Admin only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<AcademicScheduleDto>>> CreateAcademicSchedule(
            [FromForm] CreateAcademicScheduleDto dto)
        {
            try
            {
                var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(adminId))
                    return Unauthorized(ApiResponse<AcademicScheduleDto>.UnauthorizedResponse("Invalid token"));

                var command = new CreateAcademicScheduleCommand
                {
                    Dto = dto,
                    AdminId = adminId
                };

                var result = await _mediator.Send(command);

                return StatusCode(201, ApiResponse<AcademicScheduleDto>.SuccessResponse(
                    result,
                    "Academic schedule created successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<AcademicScheduleDto>.NotFoundResponse(ex.Message));
            }
            catch (Shared.Exceptions.ValidationException ex)
            {
                return BadRequest(ApiResponse<AcademicScheduleDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating academic schedule");
                return StatusCode(500, ApiResponse<AcademicScheduleDto>.ServerErrorResponse(
                    "An error occurred while creating academic schedule"));
            }
        }

        // ────────────────────────────────────────────────────────────────────────
        // DELETE /api/academic-schedules/{id}
        // ────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Delete an academic schedule (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteAcademicSchedule(int id)
        {
            try
            {
                var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(adminId))
                    return Unauthorized(ApiResponse<object>.UnauthorizedResponse("Invalid token"));

                var command = new DeleteAcademicScheduleCommand
                {
                    Id = id,
                    AdminId = adminId
                };

                await _mediator.Send(command);

                return Ok(ApiResponse<object>.SuccessResponse("Academic schedule deleted successfully"));
            }
            catch (Shared.Exceptions.NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.NotFoundResponse(ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<object>.UnauthorizedResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting academic schedule: {Id}", id);
                return StatusCode(500, ApiResponse<object>.ServerErrorResponse(
                    "An error occurred while deleting academic schedule"));
            }
        }
    }
}